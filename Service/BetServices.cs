using System.Collections.Generic;
using Core.Data;
using System.Data;
using System.Linq;
using Core.DTO.Response;
using System;
using Shared.Models;
using Data.DAL;
using System.Data.Entity;
using Shared.Common;
using System.Data.SqlClient;
using Core.DataAccess.Interface;
using Dapper;
using AutoMapper;

namespace Service
{
    public interface IBetServices : IDisposable
    {
        CRUDResult<IEnumerable<BetRes>> List();
        CRUDResult<BetRes> GetById(int id);
        CRUDResult<BetRes> GetByRoomAvailable(int roomId);
        CRUDResult<ResultBetRes> GetResultBet(int roomId);
        CRUDResult<BetRes> GetByCode(Guid code);
        CRUDResult<int> Create(BetInsertReq obj);
        CRUDResult<bool> Update(BetUpdateReq obj);

    }
    public class BetServices : IBetServices
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;
        public static Random rnd = new Random();

        public BetServices(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }
        public CRUDResult<IEnumerable<BetRes>> List()
        {
            var result = _readOnlyRepository.Value.StoreProcedureQuery<BetRes>("SP_Bet_GetAll");
            if (result == null)
            {
                return new CRUDResult<IEnumerable<BetRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<BetRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }
        public CRUDResult<BetRes> GetById(int id)
        {
            if (id <= 0) return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            BetRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<BetRes>(
                @"SELECT * FROM Bet WHERE Id = @Id",
                new
                {
                    Id = id
                });
            if (item == null)
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }

        public CRUDResult<BetRes> GetByRoomAvailable(int roomId)
        {
            if (roomId <= 0) return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            BetRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<BetRes>(
                @"SELECT * FROM Bet WHERE RoomId = @RoomId AND IsComplete = 0",
                new
                {
                    RoomId = roomId
                });
            if (item == null)
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }
        public CRUDResult<ResultBetRes> GetResultBet(int betId)
        {
            var resultTrans = _readOnlyRepository.Value.StoreProcedureQuery<TransactionRes>("SP_Transaction_GetByBetId", new { BetId = betId}).ToList();
            var dataResult = SelectItem(resultTrans);
            var param = new DynamicParameters();
            param.Add("@BetId", betId);
            param.Add("@UserIdWin", dataResult.UserId);
            var result = _readOnlyRepository.Value.StoreProcedureQuery<ResultBetRes>("SP_Bet_GetResult", param).FirstOrDefault();
            if (result != null)
            {
                return new CRUDResult<ResultBetRes>() { Data = result, StatusCode = CRUDStatusCodeRes.Success };
            }
            return new CRUDResult<ResultBetRes>() { StatusCode = CRUDStatusCodeRes.ResetContent, Data = null };
        }
        public CRUDResult<BetRes> GetByCode(Guid code)
        {
            if (code == Guid.Empty) return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            BetRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<BetRes>(
                @"SELECT * FROM Bet WHERE Code = @Code",
                new
                {
                    Code = code
                });
            if (item == null)
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<BetRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }
        public CRUDResult<int> Create(BetInsertReq obj)
        {
            var model = _repository.Value.Insert<Bet>(new Bet()
            {
                Code = obj.Code,
                RoomId = obj.RoomId,
                TotalBet = obj.TotalBet,
                CreateDate = DateTime.Now,
                IsComplete = false
            });
            return new CRUDResult<int>() { Data = model.Id, StatusCode = CRUDStatusCodeRes.ReturnWithData };
        }
        public CRUDResult<bool> Update(BetUpdateReq obj)
        {
            var getData =
                _readOnlyRepository.Value.GetById<Bet>(
                    new Bet() { Id = obj.Id });
            if (getData == null)
            {
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            var objBet = Mapper.Map<BetUpdateReq, Bet>(obj);
            objBet.TotalBet = obj.TotalBet;
            objBet.IsComplete = obj.IsComplete;
            objBet.CreateDate = getData.CreateDate;
            objBet.UpdateDate = DateTime.Now;
            _repository.Value.Update<Bet>(objBet);
            return new CRUDResult<bool>() { Data = true, StatusCode = CRUDStatusCodeRes.Success };
        }

        public void Dispose()
        {
            if (_repository.IsValueCreated)
                _repository.Value.Dispose();
            if (_readOnlyRepository.IsValueCreated)
                _readOnlyRepository.Value.Dispose();
        }

        // Static method for using from anywhere. You can make its overload for accepting not only List, but arrays also:
        // public static Item SelectItem (Item[] items)...
        public static TransactionRes SelectItem(List<TransactionRes> items)
        {
            // Calculate the summa of all portions.
            double poolSize = 0;
            for (int i = 0; i < items.Count; i++)
            {
                poolSize += (double)items[i].AmountBet;
            }

            double randomNumber = GetRandomDouble(rnd, 0, poolSize) + 1;

            // Detect the item, which corresponds to current random number.
            double accumulatedProbability = 0;
            for (int i = 0; i < items.Count; i++)
            {
                accumulatedProbability += (double)items[i].AmountBet;
                if (randomNumber <= accumulatedProbability)
                    return items[i];
            }
            return items[0];    // this code will never come while you use this programm right :)
        }

        private static double GetRandomDouble(Random random, double min, double max)
        {
            return min + (random.NextDouble() * (max - min));
        }

    }

}
