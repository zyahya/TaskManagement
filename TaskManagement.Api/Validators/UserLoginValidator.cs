using FluentValidation;

using TaskManagement.Core.Contracts.Request;

namespace TaskManagement.Api.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginValidator()
    {
        RuleFor(user => user.Username).Length(3, 14);
        RuleFor(user => user.Password).MinimumLength(8); // TODO: Make strong password constraints
    }
}
