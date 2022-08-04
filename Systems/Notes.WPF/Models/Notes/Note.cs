using System;
using AutoMapper;

namespace Notes.WPF.Models.Notes;

public class Note
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public int TaskTypeId { get; set; }
    public RepeatFrequency RepeatFrequency { get; set; }
    public TaskStatus Status { get; set; }
    public string TaskTypeColor { get; set; } = String.Empty;
}

