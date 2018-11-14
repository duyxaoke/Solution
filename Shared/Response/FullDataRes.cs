using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public class FullDataRes
    {
        public List<InfoRoomRes> InfoRooms { get; set; }
        public List<TransactionRes> InfoCharts { get; set; }
    }
}
