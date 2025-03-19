using Bacheton.Application.Reports.Common;
using MediatR;

namespace Bacheton.Application.Reports.Queries.Summary;

public record ReportsSummaryQuery(DateOnly? FromDate = null, DateOnly? ToDate = null) : IRequest<ReportSummaryResult>;