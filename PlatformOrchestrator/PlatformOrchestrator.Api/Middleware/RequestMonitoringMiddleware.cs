using System.Diagnostics;

namespace PlatformOrchestrator.Api.Middleware;

/// <summary>
/// Middleware for monitoring and telemetry
/// </summary>
public class RequestMonitoringMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestMonitoringMiddleware> _logger;

    public RequestMonitoringMiddleware(
        RequestDelegate next,
        ILogger<RequestMonitoringMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();
        
        // Add request ID to response headers for tracking
        context.Response.Headers.Add("X-Request-ID", requestId);
        
        // Add request ID to logged scopes
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["RemoteIp"] = context.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        });
        
        try
        {
            _logger.LogInformation("Request started: {Method} {Path}", 
                context.Request.Method, context.Request.Path);
            
            // Call the next middleware in the pipeline
            await _next(context);
            
            stopwatch.Stop();
            
            _logger.LogInformation("Request completed: {Method} {Path} {StatusCode} in {ElapsedMs}ms", 
                context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            _logger.LogError(ex, "Request failed: {Method} {Path} in {ElapsedMs}ms", 
                context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            
            // Re-throw to let the exception handler middleware handle it
            throw;
        }
    }
}
