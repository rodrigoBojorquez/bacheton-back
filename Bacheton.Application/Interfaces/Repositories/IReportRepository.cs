using Bacheton.Application.Common.Results;
using Bacheton.Domain.Entities;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IReportRepository : IRepository<Report>
{
    Task<ListResult<Report>> ListAsync(
        int page,
        int pageSize,
        ReportStatus? reportStatus = null,
        ReportSeverity? reportSeverity = null,
        Guid? userId = null,
        Guid? resolvedById = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        double? latitude = null,
        double? longitude = null,
        double? radiusKm = null);
}