using ProtoBuf;
using System;

namespace Shared.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]

    public class TransactionRes
    {
        public int Id { get; set; }
        public int BetId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarLink { get; set; }
        public decimal AmountBet { get; set; }
        public decimal Percent { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        //cho chart
        public decimal? y { get; set; }
        public string name { get; set; }

    }
}
