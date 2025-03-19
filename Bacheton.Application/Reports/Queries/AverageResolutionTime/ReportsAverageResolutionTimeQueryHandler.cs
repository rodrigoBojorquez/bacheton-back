using Bacheton.Application.Interfaces.Repositories;
using MediatR;

namespace Bacheton.Application.Reports.Queries.AverageResolutionTime;

public class ReportsAverageResolutionTimeQueryHandler : IRequestHandler<ReportsAverageResolutionTimeQuery, double>
{
    private readonly IReportRepository _reportRepository;

    public ReportsAverageResolutionTimeQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<double> Handle(ReportsAverageResolutionTimeQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository.GetAverageResolutionTimeAsync();
    }
}