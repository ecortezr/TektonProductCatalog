using Product.Api.Middlewares;
using System.Diagnostics;

namespace Product.Api.Middlewares
{
    public class LogResponseTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public LogResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() => {
                watch.Stop();

                TimeSpan ts = watch.Elapsed;

                string elapsedTime = String.Format(
                    "{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours,
                    ts.Minutes,
                    ts.Seconds,
                    ts.Milliseconds / 10
                );

                // Console.WriteLine("elapsedTime (HH:mm:ss.ms): " + elapsedTime);

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
public static class LogResponseTimeMiddlewareExtensions
{
    public static IApplicationBuilder UseLogResponseTime(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogResponseTimeMiddleware>();
    }
}