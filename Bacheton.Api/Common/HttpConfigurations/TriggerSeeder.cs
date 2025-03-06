using Bacheton.Infrastructure.Data;

namespace Bacheton.Api.Common.HttpConfigurations;

public static class TriggerSeeder
{
    public static async Task<WebApplication> UseTriggerSeeder(this WebApplication app)
    {
        await using (var scope = app.Services.CreateAsyncScope())
        await using (var dbContext = scope.ServiceProvider.GetRequiredService<BachetonDbContext>())
        {
            await dbContext.Database.EnsureCreatedAsync();
        }
        
        return app;
    }
}