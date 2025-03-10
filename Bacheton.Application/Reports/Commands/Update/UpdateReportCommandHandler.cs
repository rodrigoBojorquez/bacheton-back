using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Update;

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}