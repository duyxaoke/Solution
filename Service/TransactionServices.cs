﻿using System.Collections.Generic;
using Core.Data;
using Core.DTO.Response;
using System;
using System.Linq;
using Shared.Models;
using Data.DAL;
using System.Data.Entity;
using Shared.Common;
using System.Data.SqlClient;
using Core.DataAccess.Interface;
using Dapper;
using AutoMapper;
using System.Data;
using Service.CacheService;

namespace Service
{
    public interface ITransactionServices : IDisposable
    {
        CRUDResult<TestRes> Test();
        CRUDResult<IEnumerable<TransactionRes>> List();
        CRUDResult<TransactionRes> GetById(int id);
        CRUDResult<bool> Create(CreateBetInsertReq obj);
        CRUDResult<bool> Update(TransactionUpdateReq obj);
        CRUDResult<IEnumerable<TransactionRes>> GetByBet(int betId);

    }
    public class TransactionServices : ITransactionServices
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;
        private readonly ICacheProviderService _cache;

        public TransactionServices(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository, ICacheProviderService cache)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _cache = cache;
        }

        public CRUDResult<TestRes> Test()
        {
            TestRes result = null;
            using (var multi = _readOnlyRepository.Value.Connection.QueryMultiple("SP_TEST", null, commandType: CommandType.StoredProcedure))
            {
                result = multi.ReadFirstOrDefault<TestRes>();
                if (result != null)
                    result.Room = multi.ReadFirstOrDefault<RoomRes>();
            }
            if (result == null)
                return new CRUDResult<TestRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            return new CRUDResult<TestRes> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }


        public CRUDResult<IEnumerable<TransactionRes>> List()
        {
            var result = _readOnlyRepository.Value.SQLQuery<TransactionRes>(@"SELECT * FROM [Transaction]");
            if (result == null)
            {
                return new CRUDResult<IEnumerable<TransactionRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<TransactionRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }
        public CRUDResult<TransactionRes> GetById(int id)
        {
            if (id <= 0) return new CRUDResult<TransactionRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            TransactionRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<TransactionRes>(
                @"SELECT * FROM Transaction WHERE Id = @Id",
                new
                {
                    Id = id
                });
            if (item == null)
                return new CRUDResult<TransactionRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<TransactionRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }

        public CRUDResult<IEnumerable<TransactionRes>> GetByBet(int betId)
        {
            if (betId <= 0) return new CRUDResult<IEnumerable<TransactionRes>> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            var result = _readOnlyRepository.Value.SQLQuery<TransactionRes>("SELECT * FROM Transaction WHERE BetId = @BetId", new
            {
                BetId = betId
            });
            if (result == null)
            {
                return new CRUDResult<IEnumerable<TransactionRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<TransactionRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }

        public CRUDResult<bool> Create(CreateBetInsertReq obj)
        {
            #region Clear cache when insert or update
            _cache.Invalidate("InfoRoom");
            #endregion

            var param = new DynamicParameters();
            param.Add("@RoomId", obj.RoomId);
            param.Add("@UserId", obj.UserId);
            param.Add("@AmountBet", obj.AmountBet);
            _repository.Value.Connection.Execute("SP_Bet_Create", param, commandType: CommandType.StoredProcedure);
            return new CRUDResult<bool>() { Data = true, StatusCode = CRUDStatusCodeRes.Success };
        }

        public CRUDResult<bool> Update(TransactionUpdateReq obj)
        {
            var getData =
                _readOnlyRepository.Value.GetById<Transaction>(
                    new Transaction() { Id = obj.Id });
            if (getData == null)
            {
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            var objTransaction = Mapper.Map<TransactionUpdateReq, Transaction>(obj);
            objTransaction.BetId = obj.BetId;
            objTransaction.AmountBet = obj.AmountBet;
            objTransaction.UpdateDate = DateTime.Now;
            _repository.Value.Update<Transaction>(objTransaction);
            return new CRUDResult<bool>() { Data = true, StatusCode = CRUDStatusCodeRes.Success };
        }

        public void Dispose()
        {
            if (_repository.IsValueCreated)
                _repository.Value.Dispose();
            if (_readOnlyRepository.IsValueCreated)
                _readOnlyRepository.Value.Dispose();
        }

    }
}
