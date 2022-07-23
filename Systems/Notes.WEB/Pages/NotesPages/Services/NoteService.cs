using System.Text.Json;
using Notes.WEB.Pages.NotesPages.Models;

namespace Notes.WEB.Pages.NotesPages.Services;

public class NoteService : INoteService
{
    private readonly HttpClient client;

    public NoteService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IEnumerable<Note>> GetNotes()
    {
        string url = $"{Settings.ApiRoot}/Notes";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<Note>>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<Note>();

        return data;
    }
}