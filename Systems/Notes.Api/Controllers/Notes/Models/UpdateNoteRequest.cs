using AutoMapper;
using Notes.Entities;
using Notes.NotesService.Models;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class UpdateNoteRequest
{
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class UpdateNoteRequestProfile : Profile
{
    public UpdateNoteRequestProfile()
    {
        CreateMap<UpdateNoteRequest, UpdateNoteModel>();
    }
}