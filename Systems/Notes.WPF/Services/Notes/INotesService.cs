using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.WPF.Models.Notes;

namespace Notes.WPF.Services.Notes;

public interface INotesService
{
    Task<IEnumerable<Note>> GetNotes();
    Task<Note> GetNoteById(int id);
    Task AddNote(EditNote requestModel);
    Task DeleteNote(int id);
    Task UpdateNote(int id, EditNote requestModel);
    Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeks();
    Task<IEnumerable<Note>> GetNotesInInterval(DateTimeOffset start, DateTimeOffset end);
}