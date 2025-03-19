using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Reports.Common;
using MediatR;

namespace Bacheton.Application.Reports.Queries.Summary;

public class ReportsSummaryQueryHandler(IReportRepository reportRepository)
    : IRequestHandler<ReportsSummaryQuery, ReportSummaryResult>
{
    public async Task<ReportSummaryResult> Handle(ReportsSummaryQuery request, CancellationToken cancellationToken)
    {
        return await reportRepository.GetSummaryAsync(request.FromDate, request.ToDate);
    }
}