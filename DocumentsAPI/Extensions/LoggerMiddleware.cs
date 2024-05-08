namespace DocumentsAPI.Extensions;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        LogInformation("Initiated", httpContext);
        try
        {
            await _next(httpContext);
        }
        finally
        {
            LogInformation("Completed", httpContext);
        }
    }

    private void LogInformation(string text, HttpContext httpContext)
    {
        _logger.LogInformation("{Text} {Method} method on {Path}", text, httpContext.Request.Method, httpContext.Request.Path);
    }
}
