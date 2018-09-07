﻿using System;
namespace Core.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BetId { get; set; }
        public string UserId { get; set; }
        public decimal AmountBet { get; set; }
        public decimal Percent { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
        //room lay o Bet
    }
}
