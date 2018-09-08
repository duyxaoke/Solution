using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shared.Models
{
    public class CreateBetViewModel
    {
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public decimal AmountBet { get; set; }
    }

}
