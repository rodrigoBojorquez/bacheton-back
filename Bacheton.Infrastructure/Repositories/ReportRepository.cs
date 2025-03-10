using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bacheton.Infrastructure.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(BachetonDbContext context) : base(context)
    {
    }

    public async Task<ListResult<Report>> ListAsync(int page, int pageSize, ReportStatus? reportStatus = null,
        ReportSeverity? reportSeverity = null,
        Guid? userId = null, Guid? resolvedById = null, DateTime? startDate = null, DateTime? endDate = null,
        double? latitude = null, double? longitude = null, double? radiusKm = null)
    {
        IQueryable<Report> query = Context.Reports
            .Include(r => r.User)
            .Include(r => r.ResolvedBy);

        query = ApplyFilters(reportStatus, reportSeverity, userId, resolvedById, startDate, endDate, query);

        // Filtro de ubicación (Haversine Formula)
        if (latitude.HasValue && longitude.HasValue && radiusKm.HasValue)
        {
            double latRad = Math.PI * latitude.Value / 180.0;
            double lngRad = Math.PI * longitude.Value / 180.0;
            double earthRadiusKm = 6371.0;

            query = query.Where(r =>
                earthRadiusKm * Math.Acos(
                    Math.Sin(latRad) * Math.Sin(Math.PI * r.Latitude / 180.0) +
                    Math.Cos(latRad) * Math.Cos(Math.PI * r.Latitude / 180.0) *
                    Math.Cos(Math.PI * r.Longitude / 180.0 - lngRad)
                ) <= radiusKm.Value);
        }

        int totalItems = await query.CountAsync();

        var reports = await query
            .OrderByDescending(r => r.CreateDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ListResult<Report>(page, pageSize, totalItems, reports);
    }

    private static IQueryable<Report> ApplyFilters(ReportStatus? reportStatus, ReportSeverity? reportSeverity,
        Guid? userId, Guid? resolvedById,
        DateTime? startDate, DateTime? endDate, IQueryable<Report> query)
    {
        if (reportStatus.HasValue)
            query = query.Where(r => r.Status == reportStatus.Value);

        if (reportSeverity.HasValue)
            query = query.Where(r => r.Severity == reportSeverity.Value);

        if (userId.HasValue)
            query = query.Where(r => r.UserId == userId.Value);

        if (resolvedById.HasValue)
            query = query.Where(r => r.ResolvedById == resolvedById.Value);

        if (startDate.HasValue)
            query = query.Where(r => r.CreateDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(r => r.CreateDate <= endDate.Value);
        
        return query;
    }
}