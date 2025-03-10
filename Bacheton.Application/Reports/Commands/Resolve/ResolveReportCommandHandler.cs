using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Resolve;

public class ResolveReportCommandHandler : IRequestHandler<ResolveReportCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(ResolveReportCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}