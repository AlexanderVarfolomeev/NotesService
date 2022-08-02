using AutoMapper;
using FluentValidation;
using Notes.TaskTypeService.Interfaces;
using Notes.TaskTypeService.Models;
using Notes.TaskTypeService.Validators;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeRequest : ITaskTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
}
public class TaskTypeRequestProfile : Profile
{
    public TaskTypeRequestProfile()
    {
        CreateMap<TaskTypeRequest, TaskTypeRequestModel>();
    }
}

public class TaskTypeRequestValidator : AbstractValidator<TaskTypeRequest>
{
    public TaskTypeRequestValidator()
    {
        Include(new TaskTypeValidator());
    }
}