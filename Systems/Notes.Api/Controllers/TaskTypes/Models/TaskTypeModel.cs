using AutoMapper;
using Notes.TaskTypeService.Models;
using ColorTaskType = Notes.Common.Classes.ColorTaskType;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeResponseProfile : Profile
{
    public TaskTypeResponseProfile()
    {
        CreateMap<TaskTypeModel, TaskTypeResponse>();
    }
}