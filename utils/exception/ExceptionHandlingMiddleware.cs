using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, " an unhadled exception occured, traceid: {traceID}", context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }

    }

    private static async Task HandleExceptionAsync(HttpContext contex, Exception exception)
    {
        contex.Response.ContentType = "application/json";

        var response = new
        {
            Success = false,
            message = "An error occured processing your request",
            TraceId = contex.TraceIdentifier,
            TimeStamp = DateTime.Now
        };

        switch (exception)
        {
            case ValidationException validationEx:
                contex.Response.StatusCode = 400;
                response = response with { message = validationEx.Message };
                break;
            case KeyNotFoundException keyNotFoundException:
                contex.Response.StatusCode = 404;
                response = response with { message = keyNotFoundException.Message };
                break;
            case UnauthorizedAccessException:
                contex.Response.StatusCode = 401;
                response = response with { message = "Unauthorized access" };
                break;

            default:
                contex.Response.StatusCode = 500;
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        await contex.Response.WriteAsync(jsonResponse);

    }
}