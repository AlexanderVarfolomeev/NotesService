using AutoMapper;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService.Models;

public class NoteModel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public int TaskTypeId { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class NoteModelProfile : Profile
{
    public NoteModelProfile()
    {
        CreateMap<Note, NoteModel>()
            .ForPath(n => n.Type, opt => opt.MapFrom(src => src.Type.Name));
    }
}