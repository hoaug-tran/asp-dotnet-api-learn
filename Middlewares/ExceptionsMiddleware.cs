using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearnLinQWeb.Middlewares;

public class ExceptionsMiddleware
{

    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            await HandleExceptionsAsync(ctx, ex);
        }
    }

    public static Task HandleExceptionsAsync(HttpContext ctx, Exception ex)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var res = new { statusCode = ctx.Response.StatusCode, Message = "Lỗi hệ thống", Details = ex.Message, };

        return ctx.Response.WriteAsync(JsonSerializer.Serialize(res));
    }

}
