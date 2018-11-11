using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Core.DTO.Response;
using Shared.Models;
using System;
using Shared.Common;
using Data.DAL;
using Core.DataAccess.Interface;
using Dapper;
using AutoMapper;

namespace Service
{
    public interface IRoomServices : IDisposable
    {
        CRUDResult<IEnumerable<RoomRes>> List();
        CRUDResult<RoomRes> GetById(int id);
        CRUDResult<IEnumerable<InfoRoomRes>> GetInfoRooms();
        CRUDResult<int> Create(RoomInsertReq obj);
        CRUDResult<bool> Update(RoomUpdateReq obj);
        CRUDResult<bool> Delete(int id);

    }
    public class RoomServices : IRoomServices
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;
        private readonly ICacheProviderService _cache;

        public RoomServices(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository, ICacheProviderService cache)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _cache = cache;
        }
        public CRUDResult<IEnumerable<RoomRes>> List()
        {
            var result = _readOnlyRepository.Value.SQLQuery<RoomRes>("SELECT * FROM Room");
            if (result == null)
            {
                return new CRUDResult<IEnumerable<RoomRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<RoomRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
        }
        public CRUDResult<RoomRes> GetById(int id)
        {
            if (id <= 0) return new CRUDResult<RoomRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = "Dữ liệu truyền vào không hợp lệ.", Data = null, };
            RoomRes item = _readOnlyRepository.Value.Connection.QuerySingleOrDefault<RoomRes>(
                @"SELECT * FROM Room WHERE Id = @Id",
                new
                {
                    Id = id
                });
            if (item == null)
                return new CRUDResult<RoomRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            else
                return new CRUDResult<RoomRes> { StatusCode = CRUDStatusCodeRes.Success, Data = item };
        }
        private IEnumerable<InfoRoomRes> GetDatatracking()
        {
            // First, check the cache
            var trackingdata = _cache.Get("InfoRoom") as IDictionary<int?, InfoRoomRes>;

            // If it's not in the cache, we need to read it from the repository
            if (trackingdata == null)
            {
                //kiem tra thong bao trong ngay
                trackingdata = _readOnlyRepository.Value.StoreProcedureQuery<InfoRoomRes>("SP_Room_GetInfo").ToDictionary(v => v.RoomId);

                if (trackingdata.Any())
                {
                    // Put this data into the cache for 300 minutes
                    _cache.Set("InfoRoom", trackingdata); // 300 minute
                }
            }
            return trackingdata.Values;
        }

        public CRUDResult<IEnumerable<InfoRoomRes>> GetInfoRooms()
        {
            var result = GetDatatracking();
            if (result == null)
            {
                return new CRUDResult<IEnumerable<InfoRoomRes>> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            else
            {
                return new CRUDResult<IEnumerable<InfoRoomRes>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }

        }

        public CRUDResult<int> Create(RoomInsertReq obj)
        {
            var data = _repository.Value.Insert<Room>(new Room()
            {
                Name = obj.Name,
                MaxBet = obj.MaxBet,
                MinBet = obj.MinBet
            });
            return new CRUDResult<int>() { Data = data.Id, StatusCode = CRUDStatusCodeRes.ReturnWithData };
        }
        public CRUDResult<bool> Update(RoomUpdateReq obj)
        {
            var getData =
                _readOnlyRepository.Value.GetById<Room>(
                    new Room() { Id = obj.Id });
            if (getData == null)
            {
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }
            var objRoom = Mapper.Map<RoomUpdateReq, Room>(obj);
            objRoom.Name = obj.Name;
            objRoom.MaxBet = obj.MaxBet;
            objRoom.MinBet = obj.MinBet;
            _repository.Value.Update<Room>(objRoom);
            return new CRUDResult<bool>() { Data = true, StatusCode = CRUDStatusCodeRes.Success };
        }
        public CRUDResult<bool> Delete(int id)
        {
            var objData = _readOnlyRepository.Value.GetById<Test>(new Test() { Id = id });
            if (objData == null)
                return new CRUDResult<bool>() { StatusCode = CRUDStatusCodeRes.ResourceNotFound, Data = false };

            int row = _repository.Value.Connection.Execute(@"DELETE Room
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
