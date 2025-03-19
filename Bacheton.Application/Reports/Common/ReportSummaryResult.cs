namespace Bacheton.Application.Reports.Common;

public record ReportSummaryResult(int TotalReports, int PendingReports, int InProgressReports, int ResolvedReports);