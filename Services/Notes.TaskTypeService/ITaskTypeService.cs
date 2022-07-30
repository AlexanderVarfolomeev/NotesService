using Notes.TaskTypeService.Models;

namespace Notes.TaskTypeService;

public interface ITaskTypeService
{
    Task<IEnumerable<TaskTypeModel>> GetTaskTypes();
    Task<TaskTypeModel> GetTaskById(int taskId);
    Task AddTask(TaskTypeRequestModel task);
    Task UpdateTask(TaskTypeRequestModel task, int taskId);
    Task DeleteTask(int taskId);
    
}