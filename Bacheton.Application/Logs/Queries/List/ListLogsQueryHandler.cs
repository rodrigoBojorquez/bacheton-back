using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Logs.Common;
using MediatR;

namespace Bacheton.Application.Logs.Queries.List;

public class ListLogsQueryHandler : IRequestHandler<ListLogsQuery, List<LogResult>>
{
    private readonly ILogRepository _logRepository;

    public ListLogsQueryHandler(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }


    public async Task<List<LogResult>> Handle(ListLogsQuery request, CancellationToken cancellationToken)
    {
        return await _logRepository.ListAllAsync();
    }
}