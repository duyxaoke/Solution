using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace Shared.Models
{
    [Validator(typeof(BetInsertReqValidator))]

    public class BetInsertReq : BaseDTO
    {
        public Guid Code { get; set; }
        public int RoomId { get; set; }
        public string UserIdWin { get; set; }// user win vòng đó
        public decimal TotalBet { get; set; } //sum tat ca
        public bool IsComplete { get; set; }
        public DateTime? Finished { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class BetInsertReqValidator : AbstractValidator<BetInsertReq>
    {
        public BetInsertReqValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.TotalBet).NotEmpty();
        }
    }

}
