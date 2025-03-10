namespace Bacheton.Application.Reports.Common;

public record ReportResult(
    Guid Id,
    string Comment,
    string Location,
    double Latitude,
    double Longitude,
    DateTime? ResolveDate,
    string ImageUrl,
    string Status,
    string Severity,
    DateTime CreateDate,
    Guid UserId,
    string UserName,
    Guid? ResolvedById,
    string ResolvedByUserName);