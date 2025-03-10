using Bacheton.Api.Common.DependencyInjection;
using Bacheton.Api.Common.HttpConfigurations;
using Bacheton.Api.Common.Middlewares;
using Bacheton.Application.Common.DepedencyInjection;
using Bacheton.Infrastructure.Common.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();
builder.Services.AddCustomProblemDetails();
builder.Services.AddUtilities();

builder.Services.AddOpenApi(opt =>
{
    opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddControllers();

var app = builder.Build();

await app.UseTriggerSeeder();

app.UseCors(config => { config.AllowAnyMethod().AllowAnyHeader().AllowCredentials(); });
app.MapOpenApi();
app.UseSwaggerUI(config => { config.SwaggerEndpoint("/openapi/v1.json", "Bacheton API"); });

// var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images");
//
// if (!Directory.Exists(imagesPath))
// {
//     Directory.CreateDirectory(imagesPath);
// }
//
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(imagesPath),
//     RequestPath = "/images"
// });

app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseGlobalExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissionAuthorizationMiddleware>();

app.MapControllers();

app.Run();