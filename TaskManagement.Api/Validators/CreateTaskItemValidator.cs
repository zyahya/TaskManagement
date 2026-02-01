using FluentValidation;

using TaskManagement.Core.Contracts.Request;

namespace TaskManagement.Api.Validators;

public class CreateTaskItemValidator : AbstractValidator<CreateTaskItemRequest>
{
    public CreateTaskItemValidator()
    {
        RuleFor(taskItem => taskItem.Title).NotEmpty().Length(1, 100);
        RuleFor(taskItem => taskItem.Description).Length(1, 200);
        RuleFor(taskItem => taskItem.Status).IsInEnum();
    }
}
