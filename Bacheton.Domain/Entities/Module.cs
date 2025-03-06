namespace Bacheton.Domain.Entities;

public class Module
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
}