using FluentValidation;
using Notes.NotesService.Interfaces;

namespace Notes.NotesService.Validators;

public class NoteValidator : AbstractValidator<INoteRequest>
{
    public NoteValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.EndDateTime)
            .GreaterThanOrEqualTo(x => x.StartDateTime)
            .WithMessage("The final time cannot be less than the initial time.")
            .Must((model, end) => model.StartDateTime.Year == end.Year
                                  && model.StartDateTime.Month == end.Month
                                  && model.StartDateTime.Day == end.Day)
            .WithMessage("The event should start and end on the same day.");

        RuleFor(x => x.TaskTypeId)
            .NotEmpty()
            .WithMessage("Task type is required.");

        RuleFor(x => x.RepeatFrequency)
            .NotEmpty()
            .WithMessage("Repetition rate is required.")
            .IsInEnum()
            .WithMessage("Repetition must have a valid value");

        RuleFor(x => x.Description)
            .MaximumLength(600)
            .WithMessage("Description is too long.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .IsInEnum()
            .WithMessage("Status must have a valid value.");

    }
}