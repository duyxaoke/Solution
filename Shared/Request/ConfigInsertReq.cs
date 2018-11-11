using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(ConfigInsertReqValidator))]

    public class ConfigInsertReq : BaseDTO
    {
        public bool SystemEnable { get; set; }
        public string Currency { get; set; }
        public decimal? ReferalBonus { get; set; }
        public int Percent { get; set; }
    }
    public class ConfigInsertReqValidator : AbstractValidator<ConfigInsertReq>
    {
        public ConfigInsertReqValidator()
        {
            RuleFor(x => x.Currency).NotEmpty().Length(0, 20);
        }
    }

}
