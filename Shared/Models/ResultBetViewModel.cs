using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shared.Models
{
    public class ResultBetViewModel
    {
        public int RoomId { get; set; }
        public int TotalUser { get; set; }
        public int Winner { get; set; }
        public string UserName { get; set; }
        public decimal TotalBet { get; set; } //sum tat ca
        public decimal Percent { get; set; }
    }

}
