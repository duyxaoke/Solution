using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shared.Models
{
    public class InfoRoomsViewModel
    {
        public int BetId { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalUser { get; set; }
        public DateTime? Finished { get; set; }
    }

}
