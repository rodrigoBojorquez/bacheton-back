using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Reports.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Queries.List;

public class ListReportsQueryHandler : IRequestHandler<ListReportsQuery, ErrorOr<ListResult<ReportResult>>>
{
    private readonly IReportRepository _reportRepository;

    public ListReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<ErrorOr<ListResult<ReportResult>>> Handle(ListReportsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _reportRepository.ListAsync(
            request.Page,
            request.PageSize,
            request.ReportStatus,
            request.ReportSeverity,
            request.UserId,
            request.ResolvedById,
            request.StartDate,
            request.EndDate,
            request.Latitude,
            request.Longitude,
            request.RadiusKm
        );

        return new ListResult<ReportResult>(
            result.Page,
            result.PageSize,
            result.TotalItems,
            result.Items.Select(r => new ReportResult(
                r.Id, r.Comment, r.Location, r.Latitude, r.Longitude,
                r.ResolveDate, r.ImageUrl, r.Status.ToString(), r.Severity.ToString(), r.CreateDate, r.UserId,
                r.User.Name, r.ResolvedById, r.ResolvedBy?.Name)
            ).ToList());
    }
}