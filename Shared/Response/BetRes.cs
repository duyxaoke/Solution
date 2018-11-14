using ProtoBuf;
using System;

namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class BetRes
    {
        [ProtoMember(1)]public int Id { get; set; }
        [ProtoMember(2)]public Guid Code { get; set; }
        [ProtoMember(3)]public int RoomId { get; set; }
        [ProtoMember(4)]public string RoomName { get; set; }
        [ProtoMember(5)]public string UserIdWin { get; set; }
        [ProtoMember(6)]public string UserWin { get; set; }
        [ProtoMember(7)]public decimal TotalBet { get; set; } //sum tat ca
        [ProtoMember(8)]public decimal Profit { get; set; } // lãi từ phí
        [ProtoMember(9)]public bool IsComplete { get; set; }
        [ProtoMember(10)]public DateTime? CreateDate { get; set; }
        [ProtoMember(11)] public DateTime? UpdateDate { get; set; }
    }
}
