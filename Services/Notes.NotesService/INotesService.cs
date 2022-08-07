using Notes.NotesService.Models;

namespace Notes.NotesService;

public interface INotesService
{
    Task<IEnumerable<NoteModel>> GetNotes();
    Task<NoteModel> GetNoteById(int id);
    Task AddNote(NoteRequestModel requestModel);
    Task DeleteNote(int id);
    Task UpdateNote(int id, NoteRequestModel requestModel);
    Task UpdateNoteStatus();
    Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeksDictionary();
    Task<IEnumerable<NoteModel>> GetNotesInInterval(DateTimeOffset start, DateTimeOffset end);
    Task DoTask(int id);
}