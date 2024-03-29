﻿using AutoMapper;
using FluentValidation;
using Notes.Entities;
using Notes.NotesService.Interfaces;
using Notes.NotesService.Models;
using Notes.NotesService.Validators;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Api.Controllers.Notes.Models;

public class NoteRequest : INoteRequest
{
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
}

public class NoteRequestProfile : Profile
{
    public NoteRequestProfile()
    {
        CreateMap<NoteRequest, NoteRequestModel>();

    }
}

public class NoteRequestValidator : AbstractValidator<NoteRequest>
{
    public NoteRequestValidator()
    {
        Include(new NoteValidator());
    }
}