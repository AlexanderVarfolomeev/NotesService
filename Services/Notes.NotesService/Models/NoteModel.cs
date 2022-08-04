using AutoMapper;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService.Models;

public class NoteModel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; } = string.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
    public string TaskTypeColor { get; set; } = string.Empty;
}

public class NoteModelProfile : Profile
{
    public NoteModelProfile()
    {
        CreateMap<Note, NoteModel>()
            .ForPath(n => n.Type, opt => opt.MapFrom(src => src.Type.Name))
            .ForPath(n => n.TaskTypeColor, opt => opt.MapFrom(src => src.Type.Color.Code));
    }
}