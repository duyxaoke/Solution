using System.Collections.Generic;
using Core.Data;
using Core.DTO.Response;
using System;
using System.Linq;
using Shared.Models;

namespace Service
{
    public interface ITransactionServices
    {
        CRUDResult<IEnumerable<Transaction>> GetAll();
        CRUDResult<Transaction> GetById(int id);
        CRUDResult<IEnumerable<TransactionViewModel>> GetByBet(int betId);
        CRUDResult<bool> Create(Transaction model);
        void Save();
        void Dispose();

    }
    public class TransactionServices : ITransactionServices
    {
        private readonly UnitOfWork _unitOfWork;
        public TransactionServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CRUDResult<IEnumerable<Transaction>> GetAll()
        {
            var result = _unitOfWork.TransactionRepository.GetAll();
            return new CRUDResult<IEnumerable<Transaction>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<Transaction> GetById(int id)
        {
            var result = _unitOfWork.TransactionRepository.GetById(id);
            return new CRUDResult<Transaction> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<IEnumerable<TransactionViewModel>> GetByBet(int betId)
        {
            var result = _unitOfWork.TransactionRepository.GetMany(c => c.BetId == betId)
                .Select(c => new TransactionViewModel
                {
                    Id = c.Id,
                    AmountBet = c.AmountBet,
                    BetId = c.BetId,
                    CreateDate = c.CreateDate,
                    Percent = c.Percent,
                    UserId = c.UserId,
                    UserName = _unitOfWork.ApplicationUserRepository.GetById(c.UserId)?.UserName ?? string.Empty
                });
            return new CRUDResult<IEnumerable<TransactionViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Create(Transaction model)
        {
            var result = _unitOfWork.TransactionRepository.Insert(model);
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
