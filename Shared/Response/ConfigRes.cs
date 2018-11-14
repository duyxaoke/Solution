using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class ConfigRes
    {
        [ProtoMember(1)]public int Id { get; set; }
        [ProtoMember(2)]public bool SystemEnable { get; set; }
        [ProtoMember(3)]public string Currency { get; set; }
        [ProtoMember(4)]public decimal? ReferalBonus { get; set; }
        [ProtoMember(5)] public int Percent { get; set; }
    }
}
