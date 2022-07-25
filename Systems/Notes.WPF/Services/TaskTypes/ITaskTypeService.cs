using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;

namespace Notes.WPF.Services.TaskTypes;

public interface ITaskTypeService
{
    Task<IEnumerable<TaskType>> GetTaskTypes();
    Task<TaskType> GetTaskById(int taskId);
    Task AddTask(AddTaskType task);
    Task UpdateTask(UpdateTaskType task, int taskId);
    Task DeleteTask(int taskId);
}