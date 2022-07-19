using AutoMapper;
using Notes.Entities;
using Notes.NotesService.Models;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class NoteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class NoteResponseProfile : Profile
{
    public NoteResponseProfile()
    {
        CreateMap<NoteModel, NoteResponse>();
    }
}