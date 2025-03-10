using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Add;

public class AddReportCommandHandler : IRequestHandler<AddReportCommand, ErrorOr<Created>>
{
    public async Task<ErrorOr<Created>> Handle(AddReportCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}