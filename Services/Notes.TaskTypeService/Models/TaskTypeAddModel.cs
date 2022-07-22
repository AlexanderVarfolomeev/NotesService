using AutoMapper;
using FluentValidation;
using Notes.Entities;

namespace Notes.TaskTypeService.Models;

public class TaskTypeAddModel
{
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
}

public class TaskTypeAddModelProfile : Profile
{
    public TaskTypeAddModelProfile()
    {
        CreateMap<TaskTypeAddModel, TaskType>();
    }
}

public class TaskTypeAddModelValidator : AbstractValidator<TaskTypeAddModel>
{
    public TaskTypeAddModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(60)
            .WithMessage("Name is too long.");

    }
}