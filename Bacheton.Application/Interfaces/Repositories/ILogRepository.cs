using Bacheton.Application.Logs.Common;

namespace Bacheton.Application.Interfaces.Repositories;

public interface ILogRepository
{
    Task<List<LogResult>> ListAllAsync();
}