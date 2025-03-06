using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data;

namespace Bacheton.Infrastructure.Repositories;

public class ModuleRepository : GenericRepository<Module>, IModuleRepository
{
    public ModuleRepository(BachetonDbContext context) : base(context)
    {
    }
}