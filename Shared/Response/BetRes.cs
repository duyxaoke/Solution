using ProtoBuf;
using System;

namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class BetRes
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string UserIdWin { get; set; }
        public string UserWin { get; set; }
        public decimal TotalBet { get; set; } //sum tat ca
        public decimal Profit { get; set; } // lãi từ phí
        public bool IsComplete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
