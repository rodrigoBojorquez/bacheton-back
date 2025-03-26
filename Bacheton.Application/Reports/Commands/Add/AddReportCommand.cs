using Bacheton.Application.Common.Files;
using Bacheton.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Commands.Add;

public record AddReportCommand(
    string Comment,
    string Location,
    double Latitude,
    double Longitude,
    IFile Photo,
    ReportSeverity Severity) : IRequest<ErrorOr<Created>>;