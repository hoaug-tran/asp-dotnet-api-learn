using System.IO.Pipelines;
using System.Text.Json;
using LearnLinQWeb.Domain.Common;
using LearnLinQWeb.Domain.Entities;

namespace LearnLinQWeb.Middlewares;

public class ResponsesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponsesMiddleware>  _logger;

    public ResponsesMiddleware(RequestDelegate next, ILogger<ResponsesMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var originalBody = ctx.Response.Body;
        var memoryStream = new MemoryStream();

        ctx.Response.Body = memoryStream;

        try
        {
            await _next(ctx);

            if (ctx.Response.StatusCode < 200 || ctx.Response.StatusCode >= 300)
            {
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBody);
                return;
            }

            memoryStream.Position = 0;
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            var responseData = string.IsNullOrEmpty(responseBody)
                ? null
                : JsonSerializer.Deserialize<object>(responseBody);

            var wrapper = new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = responseData
            };

            var json = JsonSerializer.Serialize(wrapper);

            ctx.Response.ContentType = "application/json";
            ctx.Response.ContentLength = null;

            memoryStream.SetLength(0);
            await using var writer = new StreamWriter(memoryStream);
            await writer.WriteAsync(json);
            await writer.FlushAsync();

            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBody);
        }
        finally
        {
            ctx.Response.Body = originalBody;
            memoryStream.Dispose();
        }
    }

}
