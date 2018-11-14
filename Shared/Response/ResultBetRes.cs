using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class ResultBetRes
    {
        [ProtoMember(1)]public int BetId { get; set; }
        [ProtoMember(2)]public int RoomId { get; set; }
        [ProtoMember(3)]public int TotalUser { get; set; }
        [ProtoMember(4)]public string UserIdWin { get; set; }
        [ProtoMember(5)]public string UserName { get; set; }
        [ProtoMember(6)]public string AvatarLink { get; set; }
        [ProtoMember(7)]public decimal TotalBet { get; set; } //sum tat ca
        [ProtoMember(8)] public decimal Percent { get; set; }
    }
}
