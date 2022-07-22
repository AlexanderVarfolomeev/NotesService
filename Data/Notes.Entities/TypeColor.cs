namespace Notes.Entities;

public class TypeColor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public ICollection<TaskType> TaskTypes { get; set; }
}