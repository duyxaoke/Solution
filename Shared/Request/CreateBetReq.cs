using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace Shared.Models
{
    [Validator(typeof(CreateBetInsertReqValidator))]

    public class CreateBetInsertReq : BaseDTO
    {
        public int RoomId { get; set; }
        public string UserId { get; set; }
        public decimal AmountBet { get; set; }
    }
    public class CreateBetInsertReqValidator : AbstractValidator<CreateBetInsertReq>
    {
        public CreateBetInsertReqValidator()
        {
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.AmountBet).NotNull().GreaterThan(0);
        }
    }

}
