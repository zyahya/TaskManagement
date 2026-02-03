using FluentValidation;

using TaskManagement.Application.Contracts.TaskItem;

namespace TaskManagement.Presentation.Validators;

public class CreateTaskItemValidator : AbstractValidator<CreateTaskItemRequest>
{
    public CreateTaskItemValidator()
    {
        RuleFor(taskItem => taskItem.Title).NotEmpty().Length(1, 100);
        RuleFor(taskItem => taskItem.Description).MaximumLength(200);
        RuleFor(taskItem => taskItem.Status).IsInEnum().WithMessage("'{PropertyName}' must be between 0 and 2, you entered '{PropertyValue}'.");
    }
}
