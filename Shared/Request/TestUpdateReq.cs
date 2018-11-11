using Core.DTO;
using FluentValidation;
using FluentValidation.Attributes;

namespace Shared.Models
{
    [Validator(typeof(TestUpdateReqReqValidator))]

    public class TestUpdateReq : BaseDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
    public class TestUpdateReqReqValidator : AbstractValidator<TestUpdateReq>
    {
        public TestUpdateReqReqValidator()
        {
            RuleFor(x => x.Code).NotEmpty().Length(0, 150);
        }
    }

}
