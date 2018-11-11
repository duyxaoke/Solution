using System.Collections.Generic;
using Core.Data;
using Core.DTO.Response;
using System;
using Shared.Models;
using Core.DataAccess.Interface;
using Dapper;

namespace Service
{
    public interface ITestServices : IDisposable
    {
        CRUDResult<IEnumerable<TestRes>> List();
        CRUDResult<TestRes> ReadById(int id);
        CRUDResult<int> Create(TestInsertReq obj);
        CRUDResult<bool> Update(TestUpdateReq obj);
        CRUDResult<bool> Delete(int id);
    }
    public class TestService : ITestServices
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public TestService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        public CRUDResult<IEnumerable<TestRes>> List()
        {
            var result = _readOnlyRepository.Value.SQLQuery<TestRes>("Select * from Test");
            if (result == null)
            {
                return new CRUDResult<IEnumerable<TestRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<TestRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }

        public CRUDResult<TestRes> ReadById(int id)
        {
            if (id <= 0) return new CRUDResult<TestRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            TestRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<TestRes>(
                @"Select * from Test where Id = @Id",
                new
                {
                    Id = id
                });
            if (item == null)
                return new CRUDResult<TestRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<TestRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }

        public CRUDResult<int> Create(TestInsertReq obj)
        {
            var Test = _repository.Value.Insert<Test>(new Test()
            {
                Code = obj.Code
            });
            return new CRUDResult<int>() { Data = Test.Id, StatusCode = CRUDStatusCodeRes.ReturnWithData };
        }

        public CRUDResult<bool> Update(TestUpdateReq obj)
        {
            var objData = _readOnlyRepository.Value.GetById<Test>(new Test() { Id = obj.Id });
            if (objData == null)
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound, Data = false };

            DynamicParameters parameters = obj.ToDynamicParameters();
            int row = _repository.Value.Connection.Execute(@"UPDATE Test SET
                Code=@Code
                where Id = @Id", parameters);
            return new CRUDResult<bool>() { Data = (row > 0), StatusCode = CRUDStatusCodeRes.Success };
        }

        public CRUDResult<bool> Delete(int id)
        {
            var objData = _readOnlyRepository.Value.GetById<Test>(new Test() { Id = id });
            if (objData == null)
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound, Data = false };

            int row = _repository.Value.Connection.Execute(@"DELETE Test
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
