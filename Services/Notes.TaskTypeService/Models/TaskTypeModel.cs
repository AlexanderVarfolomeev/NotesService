using AutoMapper;
using Notes.Entities;
using ColorTaskType = Notes.Common.Classes.ColorTaskType;

namespace Notes.TaskTypeService.Models;

public class TaskTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeModelProfile : Profile
{
    public TaskTypeModelProfile()
    {
        CreateMap<TaskType, TaskTypeModel>();
    }
}