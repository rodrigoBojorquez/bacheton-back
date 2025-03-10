using Bacheton.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Update;

public record UpdateReportCommand(Guid Id, string Comment, ReportSeverity Severity) : IRequest<ErrorOr<Updated>>;