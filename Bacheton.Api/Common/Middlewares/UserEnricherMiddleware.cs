using Serilog.Context;
using System.Security.Claims;
using Serilog;

namespace Bacheton.Api.Common.Middlewares // Usa tu namespace adecuado
{
    public class UserEnricherMiddleware
    {
        private readonly RequestDelegate _next;

        public UserEnricherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string userId = "Anonymous"; // Valor por defecto para usuarios no autenticados

            if (context.User.Identity?.IsAuthenticated == true)
            {
                userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? context.User.FindFirst("sub")?.Value
                         ?? context.User.FindFirst("userId")?.Value
                         ?? "UnknownUser";
            }

            // Enriquecer el log con el UserId
            LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("TraceId", context.TraceIdentifier);
            LogContext.PushProperty("Type", "Request");

            await _next(context);
            
            LogContext.PushProperty("StatusCode", context.Response.StatusCode);
        }
    }
}
