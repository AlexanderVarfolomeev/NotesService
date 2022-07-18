namespace Notes.Entities;

public class TaskType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ColorTaskType Color { get; set; }
    public ICollection<Note> Notes { get; set; }
}