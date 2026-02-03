using FluentValidation;

using TaskManagement.Application.Contracts.Authentication;

namespace TaskManagement.Presentation.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterValidator()
    {
        RuleFor(user => user.Username).Length(3, 14);
        RuleFor(user => user.Password).MinimumLength(8);
    }
}
