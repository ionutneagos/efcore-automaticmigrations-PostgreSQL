namespace EFCore.AutomaticMigrations.EF.Sample;

//this entity is mapped via fluent api
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }

}
