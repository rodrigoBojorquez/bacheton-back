using System.Linq.Expressions;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.User.Common;
using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bacheton.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User> , IUserRepository
{
    public UserRepository(BachetonDbContext context) : base(context) { }


    public Task<User?> GetByEmailAsync(string email)
    {
        return Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ListResult<User>> ListWithRoleAsync(int page = 1, int pageSize = 10, Expression<Func<User, bool>>? filter = null)
    {
        var query = Context.Users
            .Include(u => u.Role)
            .AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        var totalItems = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new ListResult<User>(page, pageSize, totalItems, items);
    }

    public async Task<User?> IncludeRoleAsync(Guid id)
    {
        return await Context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<TopUserResult>> GetTopUsersAsync()
    {
        return await Context.Users
            .OrderByDescending(u => u.Reports.Count)
            .Take(10)
            .Select(u => new TopUserResult(u.Id, u.Name, u.Email ?? string.Empty, u.Reports.Count))
            .ToListAsync();
    }
}