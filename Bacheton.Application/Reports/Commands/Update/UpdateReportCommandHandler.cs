using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Update;

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ErrorOr<Updated>>
{
    private readonly IReportRepository _reportRepository;

    public UpdateReportCommandHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIdAsync(request.Id);

        if (report is null)
            return Errors.Report.NotFound;

        report.Comment = request.Comment;
        report.Severity = request.Severity;

        await _reportRepository.UpdateAsync(report);

        return Result.Updated;
    }
}