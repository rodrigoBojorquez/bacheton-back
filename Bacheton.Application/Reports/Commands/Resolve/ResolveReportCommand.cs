using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Resolve;

public record ResolveReportCommand() : IRequest<ErrorOr<Updated>>;