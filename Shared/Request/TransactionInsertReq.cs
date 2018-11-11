using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace Shared.Models
{
    [Validator(typeof(TransactionInsertReqValidator))]

    public class TransactionInsertReq : BaseDTO
    {
        public int BetId { get; set; }
        public string UserId { get; set; }
        public decimal AmountBet { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class TransactionInsertReqValidator : AbstractValidator<TransactionInsertReq>
    {
        public TransactionInsertReqValidator()
        {
            RuleFor(x => x.BetId).NotNull().GreaterThan(0);
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.AmountBet).NotNull().GreaterThan(0);

        }
    }

}
