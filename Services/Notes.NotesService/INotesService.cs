using Notes.NotesService.Models;

namespace Notes.NotesService;

public interface INotesService
{
    Task<IEnumerable<NoteModel>> GetNotes();
    Task<NoteModel> GetNoteById(int id);
    Task AddNote(AddNoteModel model);
    Task DeleteNote(int id);
    Task UpdateNote(int id, UpdateNoteModel model);
    Task UpdateNoteStatus();
    Task<IEnumerable<NoteModel>> GetCompletedTaskForLastFourWeeks();
}