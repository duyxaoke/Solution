using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(RoomInsertReqValidator))]

    public class RoomInsertReq : BaseDTO
    {
        public string Name { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
    }
    public class RoomInsertReqValidator : AbstractValidator<RoomInsertReq>
    {
        public RoomInsertReqValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 20);
            RuleFor(x => x.MinBet).NotNull().GreaterThan(0);
            RuleFor(x => x.MaxBet).NotNull().GreaterThan(0);
        }
    }

}
