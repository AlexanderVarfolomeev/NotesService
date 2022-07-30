using AutoMapper;
using Notes.Entities;

namespace Notes.TaskTypeService.Models;

public class TaskTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
    public TypeColor Color { get; set; }
}

public class TaskTypeModelProfile : Profile
{
    public TaskTypeModelProfile()
    {
        CreateMap<TaskType, TaskTypeModel>();
    }
}