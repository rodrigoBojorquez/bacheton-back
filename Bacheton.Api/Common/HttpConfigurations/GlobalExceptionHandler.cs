using Bacheton.Infrastructure.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Context;

namespace Bacheton.Api.Common.HttpConfigurations;

public static class GlobalExceptionHandler
{
    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", (HttpContext httpContext) =>
        {
            Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception is null)
                return Results.Problem();

            LogContext.PushProperty("Type", "Error");
            LogContext.PushProperty("Message", exception.Message);

            return exception switch
            {
                IServiceException serviceException => Results.Problem(detail: serviceException.ErrorMessage,
                    statusCode: serviceException.StatusCode),
                _ => Results.Problem(detail: "\ud83d\udd34 Error interno de servidor, intente mas tarde")
            };
        });

        return app;
    }
}