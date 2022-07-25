﻿using AutoMapper;
using FluentValidation;
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
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
}

public class UpdateNoteRequestProfile : Profile
{
    public UpdateNoteRequestProfile()
    {
        CreateMap<UpdateNoteRequest, UpdateNoteModel>();
    }
}

public class UpdateNoteRequestValidator : AbstractValidator<UpdateNoteRequest>
{
    public UpdateNoteRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.EndDateTime)
            .GreaterThanOrEqualTo(x => x.StartDateTime)
            .WithMessage("The final time cannot be less than the initial time.")
            .Must((model, end) => model.StartDateTime.Year == end.Year
                                  && model.StartDateTime.Month == end.Month
                                  && model.StartDateTime.Day == end.Day)
            .WithMessage("The event should start and end on the same day.");

        RuleFor(x => x.TaskTypeId)
            .NotEmpty()
            .WithMessage("Task type is required.");

        RuleFor(x => x.RepeatFrequency)
            .NotEmpty()
            .WithMessage("Repetition rate is required.")
            .IsInEnum()
            .WithMessage("Repetition must have a valid value");

        RuleFor(x => x.Description)
            .MaximumLength(600)
            .WithMessage("Description is too long.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .IsInEnum()
            .WithMessage("Status must have a valid value");

    }
}