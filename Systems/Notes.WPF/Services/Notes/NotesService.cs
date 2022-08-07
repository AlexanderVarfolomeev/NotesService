using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notes.WPF.Models.Notes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Services.Notes;

public class NotesService : INotesService
{
    private readonly HttpClient client;
    private readonly IUserDialogService userDialogService;
    public NotesService()
    {
        client = new HttpClient();
        userDialogService = new UserDialogService();
    }

    public async Task<IEnumerable<Note>> GetNotes()
    {
        string url = $"{Settings.ApiRoot}/Notes";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
           userDialogService.ShowError("Ошибка при получени списка задач.", "Ошибка!");
        }

        var data = JsonSerializer.Deserialize<IEnumerable<Note>>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<Note>();

        return data;
    }

    public async Task DoTask(int id)
    {
        string url = $"{Settings.ApiRoot}/Notes/do-task-{id}";

        var body = "";
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(url, request);

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при выполнии задачи.", "Ошибка!");
        }
    }
    public async Task<Note> GetNoteById(int id)
    {
        string url = $"{Settings.ApiRoot}/Notes/{id}";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при получени задачи.", "Ошибка!");
        }

        var data = JsonSerializer.Deserialize<Note>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new Note();

        return data;
    }

    public async Task AddNote(EditNote requestModel)
    {
        string url = $"{Settings.ApiRoot}/Notes";


        var body = JsonSerializer.Serialize(requestModel);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, request);


        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при добавлении задачи.", "Ошибка!");
        }
    }

    public async Task DeleteNote(int id)
    {
        string url = $"{Settings.ApiRoot}/Notes/{id}";

        var response = await client.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при удалении задачи.", "Ошибка!");
        }
    }

    public async Task UpdateNote(int id, EditNote requestModel)
    {
        string url = $"{Settings.ApiRoot}/Notes/{id}";

        var body = JsonSerializer.Serialize(requestModel);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await client.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при обновлении задачи.", "Ошибка!");
        }
    }

    public async Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeks()
    {
        string url = $"{Settings.ApiRoot}/Notes/get-last-four-weeks-completed";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при получении типов задач.", "Ошибка!");
        }

        var data = JsonSerializer.Deserialize<Dictionary<string, double[]>>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new Dictionary<string, double[]>();

        return data;
    }

    public async Task<IEnumerable<Note>> GetNotesInInterval(DateTimeOffset start, DateTimeOffset end)
    {
        string url = $"{Settings.ApiRoot}/Notes/get-in-interval-{start.ToString("MM/dd/yyyyZ")}-{end.ToString("MM/dd/yyyyZ")}";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            userDialogService.ShowError("Ошибка при получении типов задач.", "Ошибка!");
        }

        var data = JsonSerializer.Deserialize<IEnumerable<Note>>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<Note>();

        return data;
    }
}