using AutoMapper;
using Notes.Entities;

namespace Notes.TaskTypeService.Models;

public class TaskTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TypeColor Color { get; set; }
}

public class TaskTypeModelProfile : Profile
{
    public TaskTypeModelProfile()
    {
        CreateMap<TaskType, TaskTypeModel>();
         //   .ForPath(n => n.Color, opt => opt.MapFrom(src => src.Color.Name)); ;
    }
}