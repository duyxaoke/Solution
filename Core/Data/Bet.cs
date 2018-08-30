using System;
namespace Core.Data
{
    public class Bet
    {
        public int Id { get; set; }
        public Guid Code { get; set; } = Guid.NewGuid();
        public int RoomId { get; set; }
        public string UserWin { get; set; }
        public decimal TotalBet { get; set; }
        public bool IsComplete { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}
