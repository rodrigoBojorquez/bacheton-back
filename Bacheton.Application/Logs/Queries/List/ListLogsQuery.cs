using Bacheton.Application.Common.Results;
using Bacheton.Application.Logs.Common;
using MediatR;

namespace Bacheton.Application.Logs.Queries.List;

public record ListLogsQuery() : IRequest<List<LogResult>>;