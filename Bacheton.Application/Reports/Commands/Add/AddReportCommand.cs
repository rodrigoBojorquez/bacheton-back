using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Add;

public record AddReportCommand() : IRequest<ErrorOr<Created>>;