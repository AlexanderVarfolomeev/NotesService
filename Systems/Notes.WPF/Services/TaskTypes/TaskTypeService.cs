using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;

namespace Notes.WPF.Services.TaskTypes;

public class TaskTypeService : ITaskTypeService
{
    private readonly HttpClient client;

    public TaskTypeService()
    {
        client = new HttpClient();
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

        var data = JsonSerializer.Deserialize<IEnumerable<TaskType>>
            (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<TaskType>();

        return data;
    }

    public async Task<TaskType> GetTaskById(int taskId)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes/{taskId}";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<TaskType>
            (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new TaskType();

        return data;
    }

    public async Task AddTask(AddTaskType task)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes";


        var body = JsonSerializer.Serialize(task);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task UpdateTask(UpdateTaskType task, int taskId)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes/{taskId}";

        var body = JsonSerializer.Serialize(task);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task DeleteTask(int taskId)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes/{taskId}";

        var response = await client.DeleteAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }
}