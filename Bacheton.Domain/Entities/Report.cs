namespace Bacheton.Domain.Entities;

public class Report
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Comment { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? ResolveDate { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public ReportSeverity Severity { get; set; } = ReportSeverity.Low;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid? ResolvedById { get; set; }
    public User ResolvedBy { get; set; } = null!;
}

public enum ReportStatus
{
    Pending,
    InProgress,
    Resolved
}

public enum ReportSeverity
{
    Low,
    Medium,
    High
}