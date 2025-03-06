namespace Bacheton.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Guid RoleId { get; set; }
    
    public Role Role { get; set; } = null!;
    public List<Report> Reports { get; set; } = [];
    public List<Report> ResolvedReports { get; set; } = [];
}