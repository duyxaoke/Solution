using ProtoBuf;
namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class TestRes
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public RoomRes Room { get; set; }

    }

}
