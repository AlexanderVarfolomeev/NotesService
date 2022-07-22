using Notes.WEB.Shared.Common;

namespace Notes.WEB.Pages.TaskTypes.Models;

public class AddTaskType
{
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
}