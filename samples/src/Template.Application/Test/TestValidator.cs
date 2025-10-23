using FluentValidation;
using Template.Application.Contract.Test.Dto;

namespace Template.Application.Test;

public class TestValidator : AbstractValidator<TestDto>
{
    public TestValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("卫材名称不能为空");
    }
}