using FluentValidation;
using Notes.TaskTypeService.Interfaces;

namespace Notes.TaskTypeService.Validators;

public class TaskTypeValidator : AbstractValidator<ITaskTypeRequest>
{
    public TaskTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(60)
            .WithMessage("Name is too long.");
    }
}