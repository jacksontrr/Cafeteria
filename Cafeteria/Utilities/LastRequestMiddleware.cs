namespace Cafeteria.Utilities
{
    public class LastRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public LastRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Cookies.Append("LastRequest", context.Request.Path);
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }

}
