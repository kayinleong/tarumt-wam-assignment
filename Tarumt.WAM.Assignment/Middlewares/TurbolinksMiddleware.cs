using Microsoft.AspNetCore.Http.Extensions;
using Tarumt.WAM.Assignment.Extensions;
using Tarumt.WAM.Assignment.Infrastructure.Services;

namespace Tarumt.WAM.Assignment.Middlewares
{
    public static class TurbolinkMiddlewareExtension
    {
        public static IApplicationBuilder UseTurbolinkMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TurbolinkMiddleware>();
        }
    }

    public class TurbolinkMiddleware(RequestDelegate requestDelegate)
    {
        public const string TurbolinksLocationHeader = "Turbolinks-Location";

        public async Task InvokeAsync(HttpContext httpContext, UserService userService)
        {
            httpContext.Response.OnStarting((state) => {
                if (state is HttpContext ctx)
                {
                    if (ctx.IsTurbolinksRequest())
                    {
                        ctx.Response.Headers.Append(TurbolinksLocationHeader, ctx.Request.GetEncodedUrl());
                    }
                }

                return Task.CompletedTask;
            }, httpContext);

            await requestDelegate(httpContext);
        }
    }
}
