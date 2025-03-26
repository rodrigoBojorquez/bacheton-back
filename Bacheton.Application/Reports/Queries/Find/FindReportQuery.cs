using Bacheton.Application.Reports.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Reports.Queries.Find;

public record FindReportQuery(Guid Id) : IRequest<ErrorOr<ReportResult>>;