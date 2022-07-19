using AutoMapper;
using Notes.Entities;
using ColorTaskType = Notes.Common.Classes.ColorTaskType;

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