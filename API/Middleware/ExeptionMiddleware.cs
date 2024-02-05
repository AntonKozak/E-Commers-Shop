using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExeptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExeptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    //RequestDelegate is a delegate that represents the next middleware in the pipeline. Moving to the next middleware is done by invoking the delegate.
    public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, ex.Message);

            // Set the response details for an internal server error
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create an ApiException based on the development environment
            // If the environment is development, the ApiException will include the exception message and stack trace
            // If the environment is not development, the ApiException will only include the status code 
            var response = _env.IsDevelopment()
                ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }

}
