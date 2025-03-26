using Bacheton.Application.Common.Results;
using Bacheton.Application.Reports.Common;
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
    
    Task<ReportSummaryResult> GetSummaryAsync(DateOnly? startDate = null, DateOnly? endDate = null);
    
    Task<SeverityCountResult> GetSeverityAsync(DateOnly? startDate = null, DateOnly? endDate = null);
    
    /*
     * Calcula el tiempo promedio de resolusion en dias de los reportes
     */
    Task<double> GetAverageResolutionTimeAsync(DateOnly? startDate = null, DateOnly? endDate = null);
}