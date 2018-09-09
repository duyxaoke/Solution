using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shared.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public int BetId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public decimal AmountBet { get; set; }
        public decimal Percent { get; set; }
        public DateTime? CreateDate { get; set; }
        //cho chart
        public decimal? y { get; set; }
        public string name { get; set; }
    }

}
