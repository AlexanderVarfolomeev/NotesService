using Notes.NotesService.Models;

namespace Notes.NotesService;

public interface INotesService
{
    Task<IEnumerable<NoteModel>> GetNotes();
}