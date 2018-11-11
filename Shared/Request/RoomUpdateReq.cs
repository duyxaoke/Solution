using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(RoomUpdateReqReqValidator))]

    public class RoomUpdateReq : BaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MinBet { get; set; }
        public decimal MaxBet { get; set; }
    }
    public class RoomUpdateReqReqValidator : AbstractValidator<RoomUpdateReq>
    {
        public RoomUpdateReqReqValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(0, 20);
            RuleFor(x => x.MinBet).NotNull().GreaterThan(0);
            RuleFor(x => x.MaxBet).NotNull().GreaterThan(0);
        }
    }

}
