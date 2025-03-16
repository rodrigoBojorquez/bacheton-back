using Serilog.Context;
using System.Security.Claims;

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

            // Verificar si el usuario está autenticado
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // Aquí intentamos obtener el UserId del claim más común
                userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? context.User.FindFirst("sub")?.Value
                         ?? context.User.FindFirst("userId")?.Value
                         ?? "UnknownUser";
            }

            // Enriquecer el contexto del log con el UserId
            LogContext.PushProperty("UserId", userId);

            await _next(context); // Continuar con el pipeline
        }
    }
}
