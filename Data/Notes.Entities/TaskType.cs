namespace Notes.Entities;

public class TaskType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TypeColorId { get; set; }
    public TypeColor Color { get; set; }
    public ICollection<Note> Notes { get; set; }
}