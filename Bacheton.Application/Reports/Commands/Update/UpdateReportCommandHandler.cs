using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Domain.Entities;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Update;

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ErrorOr<Updated>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IAuthUtilities _authUtilities;

    public UpdateReportCommandHandler(IReportRepository reportRepository, IAuthUtilities authUtilities)
    {
        _reportRepository = reportRepository;
        _authUtilities = authUtilities;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);

        if (report is null)
            return Errors.Report.NotFound;

        report.Comment = request.Comment;
        report.Severity = request.Severity;
        report.Status = request.Status;

        if (request.Status == ReportStatus.Resolved)
        {
            report.ResolvedById = _authUtilities.GetUserId();
            report.ResolveDate = DateTime.UtcNow;
        }
        else
        {
            report.ResolvedById = null;
            report.ResolveDate = null;
        }

        await _reportRepository.UpdateAsync(report);

        return Result.Updated;
    }
}