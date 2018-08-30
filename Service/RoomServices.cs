using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Core.DTO.Response;

namespace Service
{
    public interface IRoomServices
    {
        CRUDResult<IEnumerable<Room>> GetAll();
        CRUDResult<Room> GetById(int id);
        CRUDResult<bool> Create(Room model);
        CRUDResult<bool> Update(Room model);
        CRUDResult<bool> Delete(int id);
        void Save();
        void Dispose();

    }
    public class RoomServices : IRoomServices
    {
        private readonly UnitOfWork _unitOfWork;
        public RoomServices(UnitOfWork unitOfWork)
        {
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
