using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Resolve;

public record ResolveReportCommand(Guid Id) : IRequest<ErrorOr<Updated>>;