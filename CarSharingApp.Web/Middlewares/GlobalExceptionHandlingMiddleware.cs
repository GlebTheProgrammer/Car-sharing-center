using System.Net;
using System.Security.Policy;

namespace CarSharingApp.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            switch (context.Response.StatusCode)
            {
                case 401:
                    context.Response.Redirect("https://localhost:44362/CustomExceptionHandle/Unauthorized401Error");
                    break;
                default:
                    break;
            }

        }
    }
}
