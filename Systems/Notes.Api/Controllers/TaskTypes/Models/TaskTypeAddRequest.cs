using AutoMapper;
using Notes.Entities;
using Notes.TaskTypeService.Models;
using ColorTaskType = Notes.Common.Classes.ColorTaskType;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeAddRequest
{
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeAddRequestProfile : Profile
{
    public TaskTypeAddRequestProfile()
    {
        CreateMap<TaskTypeAddRequest, TaskTypeAddModel>();
    }
}