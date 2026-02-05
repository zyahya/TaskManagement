using FluentValidation;

using TaskManagement.Application.Contracts.Authentication;


namespace TaskManagement.Presentation.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginValidator()
    {
        RuleFor(user => user.Username).Length(3, 14);
        RuleFor(user => user.Password).Length(8, 100);
    }
}
