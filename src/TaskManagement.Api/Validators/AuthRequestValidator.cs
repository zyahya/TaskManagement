using FluentValidation;

using TaskManagement.Application.Contracts.Authentication;

namespace TaskManagement.Api.Validators;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
