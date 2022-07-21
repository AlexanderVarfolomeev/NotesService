using System.Text.Json;
using Notes.WEB.Pages.TaskTypes.Models;

namespace Notes.WEB.Pages.TaskTypes.Services;

public class TaskTypeService : ITaskTypeService
{
    private readonly HttpClient client;

    public TaskTypeService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IEnumerable<TaskType>> GetTaskTypes()
    {
        string url = $"{Settings.ApiRoot}/TaskTypes";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<TaskType>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<TaskType>();

        return data;
    }

    public Task<TaskType> GetTaskById(int taskId)
    {
        throw new NotImplementedException();
    }

    public Task AddTask(AddTaskType task)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTask(UpdateTaskType task, int taskId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTask(int taskId)
    {
        throw new NotImplementedException();
    }
}