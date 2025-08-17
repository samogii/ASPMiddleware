namespace ASPHomeWork.Middleware
{
    public class HackMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            
            var query = context.Request.Query["hack"];
            if (!string.IsNullOrWhiteSpace(query))
            {
                await context.Response.WriteAsync("You can not hack this site");
                return;
            }
            await next(context);
        }
    }
}
