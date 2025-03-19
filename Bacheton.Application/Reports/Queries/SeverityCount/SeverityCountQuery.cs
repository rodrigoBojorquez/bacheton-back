using Bacheton.Application.Reports.Common;
using MediatR;

namespace Bacheton.Application.Reports.Queries.SeverityCount;

public record SeverityCountQuery() : IRequest<SeverityCountResult>;