﻿using Notes.WEB.Shared.Common;
using TaskStatus = Notes.WEB.Shared.Common.TaskStatus;

namespace Notes.WEB.Pages.NotesPages.Models;

public class Note
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string? Description { get; set; } = String.Empty;
    public string Type { get; set; }
    public RepetitionRate RepetitionRate { get; set; }
    public TaskStatus Status { get; set; }
}
