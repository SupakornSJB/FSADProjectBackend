using System.Diagnostics;
using FSADProjectBackend.Models;
using FSADProjectBackend.Services.AuditLog;
using MongoDB.Bson;

public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuditLogService _auditLogService;

    public AuditLoggingMiddleware(RequestDelegate next, IAuditLogService auditLogService)
    {
        _next = next;
        _auditLogService = auditLogService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);

        stopwatch.Stop();
        try
        {
            var user = context.User;

            var audit = new AuditLog
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Method = context.Request.Method,
                Path = context.Request.Path,
                QueryParams = context.Request.QueryString.HasValue
                    ? context.Request.QueryString.Value
                    : null,
                StatusCode = context.Response.StatusCode,
                DurationMs = (int)stopwatch.ElapsedMilliseconds,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                CreatedAt = DateTime.UtcNow
            };

            if (user?.Identity?.IsAuthenticated == true)
            {
                audit.Email = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                audit.UserSubject = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }

            await _auditLogService.CreateAsync(audit);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
