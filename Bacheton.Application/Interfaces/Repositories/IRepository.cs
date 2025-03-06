using System.Linq.Expressions;
using Bacheton.Application.Common.Results;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<ListResult<T>> ListAllAsync();
    Task<ListResult<T>> ListAsync(int page = 1, int pageSize = 10, Expression<Func<T, bool>>? filter = null);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}