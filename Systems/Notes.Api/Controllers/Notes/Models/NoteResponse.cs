using AutoMapper;
using Notes.Entities;
using Notes.NotesService.Models;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class NoteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
    public string TaskTypeColor { get; set; } = string.Empty;
}

public class NoteResponseProfile : Profile
{
    public NoteResponseProfile()
    {
        CreateMap<NoteModel, NoteResponse>()
            .AfterMap((s, d) => d.StartDateTime = s.StartDateTime.LocalDateTime)
            .AfterMap((s, d) => d.EndDateTime = s.EndDateTime.LocalDateTime); ;
    }
}