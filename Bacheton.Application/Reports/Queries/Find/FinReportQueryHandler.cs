using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Reports.Common;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Queries.Find;

public class FinReportQueryHandler : IRequestHandler<FindReportQuery, ErrorOr<ReportResult>>
{
    private readonly IReportRepository _reportRepository;

    public FinReportQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<ErrorOr<ReportResult>> Handle(FindReportQuery request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);

        if (report is null)
            return Errors.Report.NotFound;

        return new ReportResult(
            report.Id, report.Comment, report.Location, report.Latitude, report.Longitude,
            report.ResolveDate, report.ImageUrl, report.Status.ToString(), report.Severity.ToString(),
            report.CreateDate, report.UserId,
            report.User.Name, report.ResolvedById, report.ResolvedBy?.Name
        );
    }
}