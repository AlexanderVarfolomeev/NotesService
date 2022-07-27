using AutoMapper;
using Notes.Entities;
using Notes.TaskTypeService.Models;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TaskTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
    public TypeColor Color { get; set; }
}

public class TaskTypeResponseProfile : Profile
{
    public TaskTypeResponseProfile()
    {
        CreateMap<TaskTypeModel, TaskTypeResponse>();
    }
}