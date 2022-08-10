using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Services.TaskTypes;

public class TaskTypeService : ITaskTypeService
{
    private readonly HttpClient client;
    private readonly IUserDialogService userDialogService;
    public TaskTypeService(IUserDialogService userDialogService, HttpClient client)
    {
        this.userDialogService = userDialogService;
        this.client = client;
    }

    public async Task<IEnumerable<TaskType>> GetTaskTypes()
    {
        string url = $"{Settings.ApiRoot}/TaskTypes";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при получении типов задач", "Ошибка!");
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
            userDialogService.ShowError("Ошибка при получении типа задачи", "Ошибка!");
        }

        var data = JsonSerializer.Deserialize<TaskType>
            (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new TaskType();

        return data;
    }

    public async Task AddTask(EditTaskType task)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes";


        var body = JsonSerializer.Serialize(task);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при добавлении типа задачи", "Ошибка!");
        }
    }

    public async Task UpdateTask(EditTaskType task, int taskId)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes/{taskId}";

        var body = JsonSerializer.Serialize(task);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(url, request);

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при обновлении типа задачи", "Ошибка!");
        }
    }

    public async Task DeleteTask(int taskId)
    {
        string url = $"{Settings.ApiRoot}/TaskTypes/{taskId}";

        var response = await client.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при удалении типа задачи", "Ошибка!");
        }
    }
}