using AutoMapper;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.ColorService.Models;

public class ColorModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}

public class ColorModelProfile : Profile
{
    public ColorModelProfile()
    {
        CreateMap<TypeColor, ColorModel>();
    }
}