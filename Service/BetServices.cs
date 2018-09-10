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

namespace Service
{
    public interface IBetServices
    {
        CRUDResult<IEnumerable<BetViewModel>> GetAll();
        CRUDResult<Bet> GetById(int id);
        CRUDResult<Bet> GetByRoomAvailable(int roomId);
        CRUDResult<ResultBetViewModel> GetResultBet(int roomId);
        CRUDResult<Bet> GetByCode(Guid code);
        CRUDResult<bool> Create(Bet model);
        CRUDResult<bool> Update(Bet model);
        void Save();
        void Dispose();

    }
    public class BetServices : IBetServices
    {
        private readonly UnitOfWork _unitOfWork;
        public static Random rnd = new Random();
        private DatabaseContext _context;

        public BetServices(UnitOfWork unitOfWork, DatabaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public CRUDResult<IEnumerable<BetViewModel>> GetAll()
        {
            var result = _unitOfWork.BetRepository.GetAll()
                .Select(c => new BetViewModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    RoomId = c.RoomId,
                    RoomName = _unitOfWork.RoomRepository.GetById(c.RoomId)?.Name ?? string.Empty,
                    UserIdWin = c.UserIdWin,
                    UserWin = _unitOfWork.ApplicationUserRepository.GetById(c.UserIdWin)?.UserName ?? string.Empty,
                    TotalBet = c.TotalBet,
                    IsComplete = c.IsComplete,
                    CreateDate = c.CreateDate,
                    UpdateDate = c.UpdateDate
                }); ;
            return new CRUDResult<IEnumerable<BetViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<Bet> GetById(int id)
        {
            var result = _unitOfWork.BetRepository.GetById(id);
            return new CRUDResult<Bet> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<Bet> GetByRoomAvailable(int roomId)
        {
            var result = _unitOfWork.BetRepository.Get(c => c.RoomId == roomId && c.IsComplete == false);
            return new CRUDResult<Bet> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<ResultBetViewModel> GetResultBet(int betId)
        {
            var result = new ResultBetViewModel();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var lsTrans = _unitOfWork.TransactionRepository.GetMany(c=> c.BetId == betId).ToList();
                    var data = SelectItem(lsTrans);
                    SqlParameter param1 = new SqlParameter("BetId", betId);
                    SqlParameter param2 = new SqlParameter("UserIdWin", data.UserId);
                    result = _context.Database.SqlQuery<ResultBetViewModel>("EXEC SP_GetResultBet @BetId, @UserIdWin", param1, param2).FirstOrDefault();
                    dbContextTransaction.Commit();
                    return new CRUDResult<ResultBetViewModel> { StatusCode = CRUDStatusCodeRes.Success, Data = result };

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); //Required according to MSDN article 
                    return new CRUDResult<ResultBetViewModel> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result, ErrorMessage = ex.Message };

                }
            }
        }
        public CRUDResult<Bet> GetByCode(Guid code)
        {
            var result = _unitOfWork.BetRepository.Get(c=> c.Code == code);
            return new CRUDResult<Bet> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Create(Bet model)
        {
            var result = _unitOfWork.BetRepository.Insert(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Update(Bet model)
        {
            if (model.IsComplete)
            {
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
            }
            model.UpdateDate = DateTime.Now;
            var result = _unitOfWork.BetRepository.Update(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        // Static method for using from anywhere. You can make its overload for accepting not only List, but arrays also:
        // public static Item SelectItem (Item[] items)...
        public static Transaction SelectItem(List<Transaction> items)
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
