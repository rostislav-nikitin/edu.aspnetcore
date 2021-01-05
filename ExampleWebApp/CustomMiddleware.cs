using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync("<html><body>");
        await context.Response.WriteAsync("<h1>Custom Middleware :: Executed</h1>");
        await _next.Invoke(context);
        await context.Response.WriteAsync("</body></html>");
    }

}
