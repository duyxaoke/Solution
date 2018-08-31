using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Core.DTO.Response;
using System;
using Shared.Models;

namespace Service
{
    public interface IBetServices
    {
        CRUDResult<IEnumerable<BetViewModel>> GetAll();
        CRUDResult<Bet> GetById(int id);
        CRUDResult<Bet> GetByCode(Guid code);
        CRUDResult<bool> Create(Bet model);
        CRUDResult<bool> Update(Bet model);
        void Save();
        void Dispose();

    }
    public class BetServices : IBetServices
    {
        private readonly UnitOfWork _unitOfWork;
        public BetServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    Profit = c.Profit,
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
    }
}
