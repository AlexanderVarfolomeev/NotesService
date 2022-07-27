using System.Collections.Generic;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors.Models;

namespace Notes.WPF.Services.Colors;

public interface IColorService
{
    Task<IEnumerable<ColorResponse>> GetColors();
}