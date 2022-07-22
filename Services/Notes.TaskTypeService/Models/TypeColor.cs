using AutoMapper;
using Notes.Entities;

namespace Notes.TaskTypeService.Models;

public class TypeColor
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class TypeColorProfile : Profile
{
    public TypeColorProfile()
    {
        CreateMap<Entities.TypeColor, TypeColor>();
    }
}