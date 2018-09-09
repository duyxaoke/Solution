using System.Collections.Generic;
using Core.Data;
using Core.DTO.Response;
using System;
using System.Linq;
using Shared.Models;
using Data.DAL;
using System.Data.Entity;
using Shared.Common;

namespace Service
{
    public interface ITransactionServices
    {
        CRUDResult<IEnumerable<Transaction>> GetAll();
        CRUDResult<Transaction> GetById(int id);
        CRUDResult<IEnumerable<TransactionViewModel>> GetInfoChartsByRoom(int roomId);
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
        public CRUDResult<IEnumerable<TransactionViewModel>> GetInfoChartsByRoom(int roomId)
        {
            var result = new List<TransactionViewModel>();
            var bet = _unitOfWork.BetRepository.Get(c => c.RoomId == roomId && c.IsComplete == false);
            if (bet != null)
            {
                var trans = _unitOfWork.TransactionRepository.GetMany(c => c.BetId == bet.Id);
                if (trans != null)
                {
                    foreach (var item in trans)
                    {
                        var data = new TransactionViewModel();
                        data.Id = item.Id;
                        data.BetId = item.BetId;
                        data.UserId = item.UserId;
                        data.name = data.UserName = _unitOfWork.ApplicationUserRepository.GetById(item.UserId).UserName;
                        data.y = data.Percent = item.Percent;
                        data.AmountBet = item.AmountBet;
                        data.CreateDate = item.CreateDate;
                        //cho chart
                        
                        result.Add(data);
                    }
                }
            }
            return new CRUDResult<IEnumerable<TransactionViewModel>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
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
                    ////Kiểm tra user có đủ tiền ko?
                    var checkBalance = _context.Users.Find(model.UserId);
                    if (checkBalance == null || (model.AmountBet > checkBalance.Balance))
                    {
                        return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false, ErrorMessage = "Bạn không đủ số dư để cược" };
                    }
                    //kiểm tra đã có Bet cho phòng đó chưa?
                    var bet = _context.Bets.FirstOrDefault(c => c.RoomId == model.RoomId && c.IsComplete == false);
                    if (bet == null)
                    {
                        bet = new Bet();
                        bet.Code = Guid.NewGuid();
                        bet.RoomId = model.RoomId;
                        bet.TotalBet = model.AmountBet;
                        bet.CreateDate = DateTime.Now;
                        bet.IsComplete = false;
                        _context.Bets.Add(bet);
                    }
                    else
                    {
                        //Kiểm tra nếu user đó đã đặt rồi thì không đặt nữa
                        var checkBetIdByUser = _context.Transactions.FirstOrDefault(c => c.UserId == model.UserId && c.BetId == bet.Id);
                        // = null: user chưa cược, có thể chơi
                        if (checkBetIdByUser != null)
                        {
                            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false, ErrorMessage = "Bạn đã cược rồi" };
                        }
                        bet.TotalBet += model.AmountBet;
                        bet.UpdateDate = DateTime.Now;
                        _context.Bets.Attach(bet);
                        _context.Entry(bet).State = EntityState.Modified;
                    }
                    //Lưu bet
                    _context.SaveChanges();
                    //Trừ tiền user
                    var user = _context.Users.Find(model.UserId);
                    user.Balance -= model.AmountBet;
                    _context.Users.Attach(user);
                    _context.Entry(user).State = EntityState.Modified;

                    var tran = new Transaction();
                    tran.BetId = bet.Id;
                    tran.UserId = model.UserId;
                    tran.AmountBet = model.AmountBet;
                    tran.CreateDate = DateTime.Now;
                    _context.Transactions.Add(tran);
                    _context.SaveChanges();
                    //add new bet
                    var lsTransasction = _context.Transactions.Where(c => c.BetId == bet.Id);
                    foreach (var item in lsTransasction)
                    {
                        var trans = _context.Transactions.Find(item.Id);
                        trans.AmountBet = item.AmountBet;
                        trans.BetId = bet.Id;
                        trans.UpdateDate = DateTime.Now;
                        trans.UserId = item.UserId;
                        trans.Percent = Math.Round(item.AmountBet / lsTransasction.Sum(x => x.AmountBet) * 100, 2);
                        _context.Transactions.Attach(trans);
                        _context.Entry(trans).State = EntityState.Modified;
                    }
                    //nếu >= 2 người chơi => set time finish
                    var checkUser = _context.Transactions.Count(c => c.BetId == bet.Id);
                    if (checkUser > 1)
                    {
                        //====================================================================================================================
                        bet.Finished = DateTime.Now.AddSeconds(Command.Seconds);
                    }
                    _context.Bets.Attach(bet);
                    _context.Entry(bet).State = EntityState.Modified;

                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                    return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };

                    
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
