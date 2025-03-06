using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Infrastructure.Common.Auth;
using Bacheton.Infrastructure.Common.Logging;
using Bacheton.Infrastructure.Data;
using Bacheton.Infrastructure.Data.Seeders;
using Bacheton.Infrastructure.Repositories;
using Bacheton.Infrastructure.Services.Assets;
using Bacheton.Infrastructure.Services.Auth;
using Bacheton.Infrastructure.Services.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bacheton.Infrastructure.Common.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        LoggingProfile.Configure(config);
        services.AddJwtScheme(config);
        
        services.AddDbContext<BachetonDbContext>(opt =>
        {
            if (env.IsDevelopment())
            {
                opt.UseSqlite(config.GetConnectionString("BachetonConnection"))
                    .UseSeeding((context, _) => { Seeder.Administration.SeedAsync(context).Wait(); })
                    .UseAsyncSeeding(async (context, _, ct) => { await Seeder.Administration.SeedAsync(context); });
            }
            else
            {
                opt.UseNpgsql(config.GetConnectionString("BachetonConnection"))
                    .UseSeeding((context, _) => { Seeder.Administration.SeedAsync(context).Wait(); })
                    .UseAsyncSeeding(async (context, _, ct) => { await Seeder.Administration.SeedAsync(context); });
            }
        });

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();

        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, JwtService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}