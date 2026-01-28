namespace LearnLinQWeb.Middlewares;

public class RequestsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestsMiddleware> _logger;

    public RequestsMiddleware(RequestDelegate next, ILogger<RequestsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            var startTime = DateTime.UtcNow;
            
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogRequest(context, 500, DateTime.UtcNow - startTime, ex.Message);
                throw;
            }

            var elapsedMilliseconds = (DateTime.UtcNow - startTime).TotalMilliseconds;
            LogRequest(context, context.Response.StatusCode, TimeSpan.FromMilliseconds(elapsedMilliseconds));

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private void LogRequest(HttpContext context, int statusCode, TimeSpan duration, string? error = null)
    {
        var request = context.Request;
        var method = request.Method;
        var path = request.Path.Value;
        var statusCodeDisplay = GetStatusCodeDisplay(statusCode);
        var durationMs = (int)duration.TotalMilliseconds;

        if (error != null)
        {
            _logger.LogError($"{method} {path} {statusCodeDisplay} {durationMs}ms - {error}");
        }
        else
        {
            _logger.LogInformation($"{method} {path} {statusCodeDisplay} {durationMs}ms");
        }
    }

    private string GetStatusCodeDisplay(int statusCode)
    {
        return statusCode switch
        {
            >= 500 => $"\u001b[91m{statusCode}\u001b[0m",  
            >= 400 => $"\u001b[93m{statusCode}\u001b[0m",  
            >= 300 => $"\u001b[94m{statusCode}\u001b[0m", 
            >= 200 => $"\u001b[92m{statusCode}\u001b[0m",  
            _ => statusCode.ToString()
        };
    }
}