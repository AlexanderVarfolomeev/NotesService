using AutoMapper;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService.Models;

public class AddNoteModel
{
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class AddNoteModelProfile : Profile
{
    public AddNoteModelProfile()
    {
        CreateMap<AddNoteModel, Note>();
    }
}