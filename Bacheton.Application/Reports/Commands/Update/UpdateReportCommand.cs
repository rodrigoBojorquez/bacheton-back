using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Update;

public record UpdateReportCommand() : IRequest<ErrorOr<Updated>>;