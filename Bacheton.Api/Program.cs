using System.Text.Json.Serialization;
using Bacheton.Api.Common.DependencyInjection;
using Bacheton.Api.Common.HttpConfigurations;
using Bacheton.Api.Common.Middlewares;
using Bacheton.Application.Common.DepedencyInjection;
using Bacheton.Infrastructure.Common.DependencyInjection;
using Bacheton.Infrastructure.Common.Logging;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication();
builder.Services.AddCustomProblemDetails();
builder.Services.AddUtilities();

builder.Services.AddOpenApi(opt => { opt.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

var dataPath = Path.Combine(AppContext.BaseDirectory, "Data");
if (!Directory.Exists(dataPath))
{
    Directory.CreateDirectory(dataPath);
}

await app.UseTriggerSeeder();

app.UseCors(config =>
{
    config.WithOrigins("https://bacheton.space", "http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
});

app.MapOpenApi();
app.UseSwaggerUI(config => { config.SwaggerEndpoint("/openapi/v1.json", "Bacheton API"); });

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapStaticAssets();
app.UseGlobalExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissionAuthorizationMiddleware>();
app.UseMiddleware<UserEnricherMiddleware>();

app.UseSerilogRequestLogging(opts =>
{
    opts.MessageTemplate =
        "RequestId: {RequestId} | TraceId: {TraceId} | Type: {Type} | Method: {RequestMethod} | Path: {RequestPath} | Status: {StatusCode} | Duration: {Elapsed:0.0000} ms | UserId: {UserId}";
});

app.MapControllers();

app.Run();