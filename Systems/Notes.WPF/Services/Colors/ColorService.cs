﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Services.Colors;

public class ColorService : IColorService
{
    private readonly HttpClient client;
    public ColorService(HttpClient client)
    {
        this.client = client;
    }
    public  async Task<IEnumerable<ColorResponse>> GetColors()
    {
        string url = $"{Settings.ApiRoot}/v1/Color";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<ColorResponse>>
                       (content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new List<ColorResponse>();

        return data;
    }
}