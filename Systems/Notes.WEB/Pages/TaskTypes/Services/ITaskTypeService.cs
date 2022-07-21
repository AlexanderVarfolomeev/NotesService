using Notes.WEB.Pages.TaskTypes.Models;

namespace Notes.WEB.Pages.TaskTypes.Services;

public interface ITaskTypeService
{
    Task<IEnumerable<TaskType>> GetTaskTypes();
    Task<TaskType> GetTaskById(int taskId);
    Task AddTask(AddTaskType task);
    Task UpdateTask(UpdateTaskType task, int taskId);
    Task DeleteTask(int taskId);
}