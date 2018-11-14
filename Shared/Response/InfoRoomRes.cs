using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Shared.Models
{

    public class InfoRoomRes
    {
        public int? BetId { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public decimal? TotalBet { get; set; }
        public int? TotalUser { get; set; }
        public DateTime? Finished { get; set; }
    }
}
