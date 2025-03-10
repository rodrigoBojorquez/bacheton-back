using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Resolve;

public class ResolveReportCommandHandler : IRequestHandler<ResolveReportCommand, ErrorOr<Updated>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IAuthUtilities _authUtilities;

    public ResolveReportCommandHandler(IReportRepository reportRepository, IAuthUtilities authUtilities)
    {
        _reportRepository = reportRepository;
        _authUtilities = authUtilities;
    }

    public async Task<ErrorOr<Updated>> Handle(ResolveReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);

        if (report is null)
            return Errors.Report.NotFound;
        
        var userResolvedId = _authUtilities.GetUserId();
        
        report.ResolvedById = userResolvedId;
        report.ResolveDate = DateTime.UtcNow;
        
        await _reportRepository.UpdateAsync(report);
        
        return Result.Updated;
    }
}