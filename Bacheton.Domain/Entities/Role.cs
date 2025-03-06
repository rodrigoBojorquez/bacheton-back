namespace Bacheton.Domain.Entities;

public class Role
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public List<User> Users { get; set; } = [];
    public List<Permission> Permissions { get; set; } = [];
}