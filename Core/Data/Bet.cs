using System;
namespace Core.Data
{
    public class Bet
    {
        public int Id { get; set; }
        public Guid Code { get; set; } = Guid.NewGuid();
        public int RoomId { get; set; }
        public string UserIdWin { get; set; }// user win vòng đó
        public decimal TotalBet { get; set; } //sum tat ca
        public decimal Profit { get; set; } // lãi từ phí
        public bool IsComplete { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}
