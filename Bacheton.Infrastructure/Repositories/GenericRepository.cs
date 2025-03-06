using System.Linq.Expressions;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bacheton.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly DbSet<T> _set;
    protected readonly BachetonDbContext Context;

    public GenericRepository(BachetonDbContext context)
    {
        Context = context;
        _set = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<ListResult<T>> ListAllAsync()
    {
        var total = await _set.CountAsync();
        var data = await _set.ToListAsync();
        
        return new ListResult<T>(Page: 1, PageSize:total, TotalItems: total, Items: data);
    }

    public async Task<ListResult<T>> ListAsync(int page = 1, int pageSize = 10, Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _set;
        
        if (filter is not null)
        {
            query = query.Where(filter);
        }
        
        var total = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new ListResult<T>(page, pageSize, total, data);
    }

    public async Task InsertAsync(T entity)
    {
        await _set.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _set.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is not null)
        {
            _set.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}