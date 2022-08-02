using AutoMapper;
using FluentValidation;
using Notes.Entities;
using Notes.TaskTypeService.Interfaces;
using Notes.TaskTypeService.Validators;

namespace Notes.TaskTypeService.Models;

public class TaskTypeRequestModel : ITaskTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
}

public class TaskTypeRequestModelProfile : Profile
{
    public TaskTypeRequestModelProfile()
    {
        CreateMap<TaskTypeRequestModel, TaskType>();
    }
}

public class TaskTypeRequestModelValidator : AbstractValidator<TaskTypeRequestModel>
{
    public TaskTypeRequestModelValidator()
    {
       Include(new TaskTypeValidator());

    }
}