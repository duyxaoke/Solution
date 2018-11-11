using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(TestInsertReqValidator))]

    public class TestInsertReq : BaseDTO
    {
        public string Code { get; set; }
    }
    public class TestInsertReqValidator : AbstractValidator<TestInsertReq>
    {
        public TestInsertReqValidator()
        {
            RuleFor(x => x.Code).NotEmpty().Length(0, 150);
        }
    }

}
