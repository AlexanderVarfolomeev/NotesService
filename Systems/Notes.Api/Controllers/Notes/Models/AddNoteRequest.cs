using AutoMapper;
using FluentValidation;
using Notes.Common.Interfaces;
using Notes.Entities;
using Notes.NotesService.Models;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class AddNoteRequest : IAddNote
{
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
}

public class AddNoteRequestProfile : Profile
{
    public AddNoteRequestProfile()
    {
        CreateMap<AddNoteRequest, AddNoteModel>()
           .AfterMap((s, d) => d.StartDateTime = s.StartDateTime.UtcDateTime)
           .AfterMap((s, d) => d.EndDateTime = s.EndDateTime.UtcDateTime);
    }
}

public class AddNoteRequestValidator : AbstractValidator<AddNoteRequest>
{
    public AddNoteRequestValidator()
    {
        Include(new IAddNoteValidator());
    }
}