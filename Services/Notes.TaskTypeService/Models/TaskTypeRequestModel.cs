using AutoMapper;
using FluentValidation;
using Notes.Common.Interfaces;
using Notes.Entities;

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
       Include(new ITaskTypeRequestValidator());

    }
}