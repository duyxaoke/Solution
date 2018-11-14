using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class RoomRes
    {
        [ProtoMember(1)]public int Id { get; set; }
        [ProtoMember(2)]public string Name { get; set; }
        [ProtoMember(3)]public decimal MinBet { get; set; }
        [ProtoMember(4)] public decimal MaxBet { get; set; }
    }
}
