using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Reports.Common;
using MediatR;

namespace Bacheton.Application.Reports.Queries.SeverityCount;

public class SeverityCountQueryHandler : IRequestHandler<SeverityCountQuery, SeverityCountResult>
{
    private readonly IReportRepository _reportRepository;

    public SeverityCountQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<SeverityCountResult> Handle(SeverityCountQuery request, CancellationToken cancellationToken)
    {
        return await _reportRepository.GetSeverityAsync();
    }
}