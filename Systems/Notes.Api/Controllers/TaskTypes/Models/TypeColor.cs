using AutoMapper;

namespace Notes.Api.Controllers.TaskTypes.Models;

public class TypeColor
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class TypeColorProfile : Profile
{
    public TypeColorProfile()
    {
        CreateMap<TaskTypeService.Models.TypeColor, TypeColor>();
    }
}