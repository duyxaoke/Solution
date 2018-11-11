using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Core.DTO.Response;
using Shared.Models;
using Core.DataAccess.Interface;
using System;
using Dapper;
using AutoMapper;

namespace Service
{
    public interface IConfigServices : IDisposable
    {
        CRUDResult<IEnumerable<ConfigRes>> List();
        CRUDResult<ConfigRes> GetById(int id);
        CRUDResult<int> GetPercent();
        CRUDResult<int> Create(ConfigInsertReq obj);
        CRUDResult<bool> Update(ConfigUpdateReq obj);
        CRUDResult<bool> Delete(int id);

    }
    public class ConfigServices : IConfigServices
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public ConfigServices(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }
        public CRUDResult<IEnumerable<ConfigRes>> List()
        {
            var result = _readOnlyRepository.Value.SQLQuery<ConfigRes>("SELECT * FROM Config");
            if (result == null)
            {
                return new CRUDResult<IEnumerable<ConfigRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<ConfigRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }
        public CRUDResult<ConfigRes> GetById(int id)
        {
            if (id <= 0) return new CRUDResult<ConfigRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            ConfigRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<ConfigRes>(
                @"SELECT * FROM Config WHERE Id = @Id",
                new
                {
                    Id = id
                });
            if (item == null)
                return new CRUDResult<ConfigRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<ConfigRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }
        public CRUDResult<int> GetPercent()
        {
            ConfigRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<ConfigRes>(
                @"SELECT TOP 1 Percent FROM Config");
            if (item == null)
                return new CRUDResult<int> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<int> { StatusCode = CRUDStatusCodeRes.Success, Data = item.Percent };
        }
        public CRUDResult<int> Create(ConfigInsertReq obj)
        {
            var data = _repository.Value.Insert<Config>(new Config()
            {
                SystemEnable = obj.SystemEnable,
                Currency = obj.Currency,
                Percent = obj.Percent,
                ReferalBonus = obj.ReferalBonus
            });
            return new CRUDResult<int>() { Data = data.Id, StatusCode = CRUDStatusCodeRes.ReturnWithData };
        }
        public CRUDResult<bool> Update(ConfigUpdateReq obj)
        {
            var getData =
                _readOnlyRepository.Value.GetById<Config>(
                    new Config() { Id = obj.Id });
            if (getData == null)
            {
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            var objConfig = Mapper.Map<ConfigUpdateReq, Config>(obj);
            objConfig.Currency = obj.Currency;
            objConfig.Percent = obj.Percent;
            objConfig.ReferalBonus = obj.ReferalBonus;
            objConfig.SystemEnable = obj.SystemEnable;
            _repository.Value.Update<Config>(objConfig);
            return new CRUDResult<bool>() { Data = true, StatusCode = CRUDStatusCodeRes.Success };
        }
        public CRUDResult<bool> Delete(int id)
        {
            var objData = _readOnlyRepository.Value.GetById<Config>(new Config() { Id = id });
            if (objData == null)
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound, Data = false };

            int row = _repository.Value.Connection.Execute(@"DELETE Config
                where Id = @Id", new
            {
                Id = id
            });
            return new CRUDResult<bool>() { Data = (row > 0), StatusCode = CRUDStatusCodeRes.Success };
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
