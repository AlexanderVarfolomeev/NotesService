using AutoMapper;
using Notes.Entities;
using Notes.TaskTypeService.Models;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeUpdateRequest
{
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeUpdateRequestProfile : Profile
{
    public TaskTypeUpdateRequestProfile()
    {
        CreateMap<TaskTypeUpdateRequest, TaskTypeUpdateModel>();
    }
}