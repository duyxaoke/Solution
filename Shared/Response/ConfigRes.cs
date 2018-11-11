using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class ConfigRes
    {
        public int Id { get; set; }
        public bool SystemEnable { get; set; }
        public string Currency { get; set; }
        public decimal? ReferalBonus { get; set; }
        public int Percent { get; set; }
    }
}
