﻿using AutoMapper;
using Notes.Entities;
using RepetitionRate = Notes.Common.Classes.RepetitionRate;
using TaskStatus = Notes.Common.Classes.TaskStatus;

namespace Notes.NotesService.Models;

public class AddNoteModel
{
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}

public class AddNoteModelProfile : Profile
{
    public AddNoteModelProfile()
    {
        CreateMap<AddNoteModel, Note>()
            .ForMember(n => n.Type.Name, opts => opts.MapFrom(x => x.Type));
    }
}