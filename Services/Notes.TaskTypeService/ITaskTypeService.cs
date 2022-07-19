using Notes.TaskTypeService.Models;

namespace Notes.TaskTypeService;

public interface ITaskTypeService
{
    Task<IEnumerable<TaskTypeModel>> GetTaskTypes();
    Task<TaskTypeModel> GetTaskById(int taskId);
    Task AddTask(TaskTypeAddModel task);
    Task UpdateTask(TaskTypeUpdateModel task, int taskId);
    Task DeleteTask(int taskId);
    
}