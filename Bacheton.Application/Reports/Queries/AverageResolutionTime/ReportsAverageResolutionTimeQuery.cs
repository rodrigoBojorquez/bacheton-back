using MediatR;

namespace Bacheton.Application.Reports.Queries.AverageResolutionTime;

public record ReportsAverageResolutionTimeQuery() : IRequest<double>;