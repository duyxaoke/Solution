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

namespace Service
{
    public interface IRoomServices
    {
        CRUDResult<IEnumerable<Room>> GetAll();
        CRUDResult<Room> GetById(int id);
        CRUDResult<List<InfoRoomsViewModel>> GetInfoRooms();
        CRUDResult<bool> Create(Room model);
        CRUDResult<bool> Update(Room model);
        CRUDResult<bool> Delete(int id);
        void Save();
        void Dispose();

    }
    public class RoomServices : IRoomServices
    {
        private readonly UnitOfWork _unitOfWork;
        private DatabaseContext _context;

        public RoomServices(UnitOfWork unitOfWork, DatabaseContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;

        }
        public CRUDResult<IEnumerable<Room>> GetAll()
        {
            var result = _unitOfWork.RoomRepository.GetAll();
            return new CRUDResult<IEnumerable<Room>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<Room> GetById(int id)
        {
            var result = _unitOfWork.RoomRepository.GetById(id);
            return new CRUDResult<Room> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<List<InfoRoomsViewModel>> GetInfoRooms()
        {
            var result = new List<InfoRoomsViewModel>();
            try
            {
                result = _context.Database.SqlQuery<InfoRoomsViewModel>("EXEC SP_GetInfoRooms").ToList();
                return new CRUDResult<List<InfoRoomsViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
            catch (Exception ex)
            {
               return new CRUDResult<List<InfoRoomsViewModel>> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result, ErrorMessage = ex.Message };
            }
        }
        public CRUDResult<bool> Create(Room model)
        {
            var result = _unitOfWork.RoomRepository.Insert(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Update(Room model)
        {
            var result = _unitOfWork.RoomRepository.Update(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Delete(int id)
        {
            var result = _unitOfWork.RoomRepository.Delete(id);
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
    }
}
