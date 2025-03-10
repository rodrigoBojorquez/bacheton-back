using Bacheton.Application.Common.Results;
using Bacheton.Application.Reports.Common;
using Bacheton.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Queries.List;

public record ListReportsQuery(
    int Page,
    int PageSize,
    ReportStatus? ReportStatus = null,
    ReportSeverity? ReportSeverity = null,
    Guid? UserId = null,
    Guid? ResolvedById = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    double? Latitude = null,
    double? Longitude = null,
    double? RadiusKm = null
) : IRequest<ErrorOr<ListResult<ReportResult>>>;
