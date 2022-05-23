namespace Auth.Services.Infrastructure;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
 
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context, ILogger<ErrorHandlerMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.Redirect("/error");
        }
    }
}