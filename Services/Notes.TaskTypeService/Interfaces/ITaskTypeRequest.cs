namespace Notes.TaskTypeService.Interfaces;

public interface ITaskTypeRequest
{
    public string Name { get; set; }
    public int TypeColorId { get; set; }
}
