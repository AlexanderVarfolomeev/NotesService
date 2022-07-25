using AutoMapper;
using FluentValidation;
using Notes.Common.Interfaces;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService.Models;

public class AddNoteModel : IAddNote
{
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
}

public class AddNoteModelProfile : Profile
{
    public AddNoteModelProfile()
    {
        CreateMap<AddNoteModel, Note>();
    }
}

public class AddNoteModelValidator : AbstractValidator<AddNoteModel>
{
    public AddNoteModelValidator()
    {
        Include(new IAddNoteValidator());
    }
}