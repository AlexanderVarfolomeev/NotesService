using Notes.ColorService.Models;

namespace Notes.ColorService;

public interface IColorService
{
    Task<IEnumerable<ColorModel>> GetColors();
    Task<ColorModel> GetColorById(int id);
}