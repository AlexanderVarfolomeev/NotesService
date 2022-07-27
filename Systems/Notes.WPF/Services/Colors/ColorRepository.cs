using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors.Models;

namespace Notes.WPF.Services.Colors;

public static class ColorRepository
{
    private static ColorService colorService;
    public static IEnumerable<ColorResponse>? Colors { get; set; }
    static ColorRepository()
    {
        colorService = new ColorService();
    }
    public static async Task GetColors()
    {
        Colors = await colorService.GetColors();
    }
}