using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class ResultBetRes
    {
        public int BetId { get; set; }
        public int RoomId { get; set; }
        public int TotalUser { get; set; }
        public string UserIdWin { get; set; }
        public string UserName { get; set; }
        public string AvatarLink { get; set; }
        public decimal TotalBet { get; set; } //sum tat ca
        public decimal Percent { get; set; }
    }
}
