using AutoMapper;
using Notes.ColorService.Models;
using Notes.Entities;

namespace Notes.Api.Controllers.Colors.Models;

public class ColorResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}

public class ColorResponseProfile : Profile
{
    public ColorResponseProfile()
    {
        CreateMap<ColorModel, ColorResponse>();
    }
}