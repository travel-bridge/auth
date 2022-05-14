using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.Services.Infrastructure;

public class SecurityHeadersAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result;
        if (result is not ViewResult)
            return;
        
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
            context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
            context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        var contentSecurityPolicy = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
        // Also consider adding upgrade-insecure-requests once you have HTTPS in place for production
        //csp += "upgrade-insecure-requests;";
        // Also an example if you need client images to be displayed from twitter
        // csp += "img-src 'self' https://pbs.twimg.com;";

        // Once for standards compliant browsers
        if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
            context.HttpContext.Response.Headers.Add("Content-Security-Policy", contentSecurityPolicy);

        // And once again for IE
        if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
            context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", contentSecurityPolicy);

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
            context.HttpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
    }
}