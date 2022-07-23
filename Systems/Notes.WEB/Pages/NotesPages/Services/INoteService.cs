using Notes.WEB.Pages.NotesPages.Models;

namespace Notes.WEB.Pages.NotesPages.Services;

public interface INoteService
{
    Task<IEnumerable<Note>> GetNotes();
    //Task<TaskType> GetTaskById(int taskId);
    //Task AddTask(AddTaskType task);
    //Task UpdateTask(UpdateTaskType task, int taskId);
    //Task DeleteTask(int taskId);
}