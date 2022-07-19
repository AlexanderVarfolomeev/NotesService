﻿using AutoMapper;
using Notes.Entities;
using ColorTaskType = Notes.Common.Classes.ColorTaskType;

namespace Notes.TaskTypeService.Models;

public class TaskTypeAddModel
{
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}

public class TaskTypeAddModelProfile : Profile
{
    public TaskTypeAddModelProfile()
    {
        CreateMap<TaskTypeAddModel, TaskType>();
    }
}