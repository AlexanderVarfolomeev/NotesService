using AutoMapper;
using FluentValidation;
using Notes.Entities;
using Notes.TaskTypeService.Models;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeAddRequest
{
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
}

public class TaskTypeAddRequestProfile : Profile
{
    public TaskTypeAddRequestProfile()
    {
        CreateMap<TaskTypeAddRequest, TaskTypeAddModel>();
    }
}

public class TaskTypeAddRequestValidator : AbstractValidator<TaskTypeAddRequest>
{
    public TaskTypeAddRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(60)
            .WithMessage("Name is too long.");

        
    }
}