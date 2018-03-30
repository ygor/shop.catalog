using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Shop.Catalog.Api.Middleware
{
    public class TraceIdentifierLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceIdentifierLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("TraceIdentifier", context.TraceIdentifier))
            {
                await _next.Invoke(context);
            }
        }
    }
}