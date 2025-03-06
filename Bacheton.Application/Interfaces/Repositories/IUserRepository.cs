using System.Linq.Expressions;
using Bacheton.Application.Common.Results;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<Domain.Entities.User>
{
    Task<Domain.Entities.User?> GetByEmailAsync(string email);

    Task<ListResult<Domain.Entities.User>> ListWithRoleAsync(int page = 1, int pageSize = 10,
        Expression<Func<Domain.Entities.User, bool>>? filter = null);
    
    Task<Domain.Entities.User?> IncludeRoleAsync(Guid id);
}