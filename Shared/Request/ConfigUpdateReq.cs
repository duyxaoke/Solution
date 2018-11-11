using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(ConfigUpdateReqReqValidator))]

    public class ConfigUpdateReq : BaseDTO
    {
        public int Id { get; set; }
        public bool SystemEnable { get; set; }
        public string Currency { get; set; }
        public decimal? ReferalBonus { get; set; }
        public int Percent { get; set; }
    }
    public class ConfigUpdateReqReqValidator : AbstractValidator<ConfigUpdateReq>
    {
        public ConfigUpdateReqReqValidator()
        {
            RuleFor(x => x.Currency).NotEmpty().Length(0, 20);
        }
    }

}
