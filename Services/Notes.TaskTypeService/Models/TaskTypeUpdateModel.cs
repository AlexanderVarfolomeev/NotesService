using AutoMapper;
using FluentValidation;
using Notes.Entities;

namespace Notes.TaskTypeService.Models;

public class TaskTypeUpdateModel
{
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeUpdateModelProfile : Profile
{
    public TaskTypeUpdateModelProfile()
    {
        CreateMap<TaskTypeUpdateModel, TaskType>();
    }
}

public class TaskTypeUpdateModelValidator : AbstractValidator<TaskTypeUpdateModel>
{
    public TaskTypeUpdateModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(60)
            .WithMessage("Name is too long.");

        RuleFor(x => x.Color)
            .IsInEnum()
            .WithMessage("The color must have a valid value.");
    }
}