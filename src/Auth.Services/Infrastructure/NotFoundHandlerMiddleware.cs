namespace Auth.Services.Infrastructure;

public class NotFoundHandlerMiddleware
{
    private readonly RequestDelegate _next;
 
    public NotFoundHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            context.Request.Path = "/not-found";
            await _next(context);
        }
    }
}