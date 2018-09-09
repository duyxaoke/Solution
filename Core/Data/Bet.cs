using System;
namespace Core.Data
{
    public class Bet
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public int RoomId { get; set; }
        public string UserIdWin { get; set; }// user win vòng đó
        public decimal TotalBet { get; set; } //sum tat ca
        public bool IsComplete { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
