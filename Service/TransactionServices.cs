using System.Collections.Generic;
using Core.Data;
using Core.DTO.Response;
using System;
using System.Linq;
using Shared.Models;
using Data.DAL;
using System.Data.Entity;
using Shared.Common;
using System.Data.SqlClient;

namespace Service
{
    public interface ITransactionServices
    {
        CRUDResult<IEnumerable<Transaction>> GetAll();
        CRUDResult<Transaction> GetById(int id);
        CRUDResult<List<TransactionViewModel>> GetInfoChartsByRoom(int roomId);
        CRUDResult<IEnumerable<Transaction>> GetByBet(int betId);
        CRUDResult<bool> Create(CreateBetViewModel model);
        CRUDResult<bool> Update(Transaction model);
        CRUDResult<bool> Updates(List<Transaction> model);
        void Save();
        void Dispose();

    }
    public class TransactionServices : ITransactionServices
    {
        private readonly UnitOfWork _unitOfWork;
        private DatabaseContext _context;
        public TransactionServices(UnitOfWork unitOfWork, DatabaseContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
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
        public CRUDResult<List<TransactionViewModel>> GetInfoChartsByRoom(int roomId)
        {
            var result = new List<TransactionViewModel>();
            try
            {
                SqlParameter param1 = new SqlParameter("RoomId", roomId);
                result = _context.Database.SqlQuery<TransactionViewModel>("EXEC SP_GetInfoChartsByRoom @RoomId", param1).ToList();
                return new CRUDResult<List<TransactionViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
            }
            catch (Exception ex)
            {
                return new CRUDResult<List<TransactionViewModel>> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = result, ErrorMessage = ex.Message };
            }
        }
        public CRUDResult<IEnumerable<Transaction>> GetByBet(int betId)
        {
            var result = _unitOfWork.TransactionRepository.GetMany(c => c.BetId == betId);
            return new CRUDResult<IEnumerable<Transaction>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Create(CreateBetViewModel model)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    SqlParameter RoomId = new SqlParameter("RoomId", model.RoomId);
                    SqlParameter UserId = new SqlParameter("UserId", model.UserId);
                    SqlParameter AmountBet = new SqlParameter("AmountBet", model.AmountBet);
                    var result =_context.Database.SqlQuery<bool>("EXEC SP_CreateBet @RoomId, @UserId, @AmountBet", RoomId, UserId, AmountBet).FirstOrDefault();
                    dbContextTransaction.Commit();
                    return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback(); //Required according to MSDN article 
                    return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false, ErrorMessage = ex.Message };

                }
            }
        }
        public CRUDResult<bool> Update(Transaction model)
        {
            var result = _unitOfWork.TransactionRepository.Update(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Updates(List<Transaction> model)
        {
            var result = _unitOfWork.TransactionRepository.Update(model);
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
