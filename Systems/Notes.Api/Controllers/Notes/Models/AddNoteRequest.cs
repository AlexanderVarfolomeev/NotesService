using AutoMapper;
using Notes.Entities;
using Notes.NotesService.Models;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class AddNoteRequest
{
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class AddNoteRequestProfile : Profile
{
    public AddNoteRequestProfile()
    {
        CreateMap<AddNoteRequest, AddNoteModel>();
    }
}