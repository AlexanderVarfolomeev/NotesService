using AutoMapper;
using FluentValidation;
using Notes.Entities;
using Notes.NotesService.Interfaces;
using Notes.NotesService.Validators;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService.Models;

public class NoteRequestModel : INoteRequest
{
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
}

public class NoteRequestModelProfile : Profile
{
    public NoteRequestModelProfile()
    {
        CreateMap<NoteRequestModel, Note>();
    }
}

public class NoteRequestModelValidator : AbstractValidator<NoteRequestModel>
{
    public NoteRequestModelValidator()
    {
        Include(new NoteValidator());
    }
}