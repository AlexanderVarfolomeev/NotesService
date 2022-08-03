using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.WPF.Models.Notes;

namespace Notes.WPF.Services.Notes;

public interface INotesService
{
    Task<IEnumerable<Note>> GetNotes();
    Task<Note> GetNoteById(int id);
    Task AddNote(NoteRequest requestModel);
    Task DeleteNote(int id);
    Task UpdateNote(int id, NoteRequest requestModel);
    Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeks();
}