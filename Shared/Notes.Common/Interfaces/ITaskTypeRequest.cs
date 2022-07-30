using FluentValidation;

namespace Notes.Common.Interfaces;

public interface ITaskTypeRequest
{
    public string Name { get; set; }
    public int TypeColorId { get; set; }
}

public class ITaskTypeRequestValidator : AbstractValidator<ITaskTypeRequest>
{
    public ITaskTypeRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(60)
            .WithMessage("Name is too long.");

    }
}