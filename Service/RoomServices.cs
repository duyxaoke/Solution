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

namespace Service
{
    public interface IRoomServices
    {
        CRUDResult<IEnumerable<Room>> GetAll();
        CRUDResult<Room> GetById(int id);
        CRUDResult<IEnumerable<InfoRoomsViewModel>> GetInfoRooms();
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
        public CRUDResult<IEnumerable<InfoRoomsViewModel>> GetInfoRooms()
        {
            var rooms = _unitOfWork.RoomRepository.GetAll();
            var result = new List<InfoRoomsViewModel>();
            foreach (var item in rooms)
            {
                var data = new InfoRoomsViewModel();
                data.RoomId = item.Id;
                data.RoomName = item.Name;
                data.TotalAmount = 0;
                data.TotalUser = 0;
               //select room chưa chạy xong
               var betInRoom = _unitOfWork.BetRepository.Get(c => c.RoomId == item.Id && c.IsComplete == false);
                if (betInRoom != null)
                {
                    data.BetId = betInRoom.Id;
                    data.TotalAmount = Math.Round(betInRoom.TotalBet * (100 - Command.Percent) / 100, 2);
                    data.Finished = betInRoom.Finished;
                    var transInBet = _unitOfWork.TransactionRepository.GetMany(c => c.BetId == betInRoom.Id);
                    if (transInBet != null)
                    {
                        data.TotalUser = transInBet.Count();
                    }
                }
                result.Add(data);
            }
            return new CRUDResult<IEnumerable<InfoRoomsViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
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
