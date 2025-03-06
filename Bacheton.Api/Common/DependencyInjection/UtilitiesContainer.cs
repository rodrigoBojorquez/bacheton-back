using Bacheton.Api.Utilities;
using Bacheton.Application.Interfaces.Services;

namespace Bacheton.Api.Common.DependencyInjection;

public static class UtilitiesContainer
{
    public static IServiceCollection AddUtilities(this IServiceCollection services)
    {
        services.AddScoped<IAuthUtilities, AuthUtilities>();
        
        return services;
    }
}