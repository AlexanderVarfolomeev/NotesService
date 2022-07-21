using Notes.WEB.Shared.Common;

namespace Notes.WEB.Pages.TaskTypes.Models;

public class UpdateTaskType
{
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
}