
using ASPHomeWork.Middleware;

namespace ASPHomeWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
            app.UseMiddleware<HackMiddleware>();
            app.Use(async (con, next) =>
            {
                if (con.Request.Path.Value == "/")
                {
                    await con.Response.WriteAsync("""
                        Hello to this middleware tutorial
                        you can check it with /sth or ?hack=sth
                        """);
                }
                await next(con);
            });
            app.Use(async (con, next) =>
            {
                await next(con);

                if (con.Response.StatusCode == 404 && !con.Response.HasStarted)
                {
                    con.Response.StatusCode = 404;
                    
                    con.Response.Headers["Refresh"] = "5; url=/";
                    await con.Response.WriteAsync($"The {con.Request.Path.Value} is not avalible in our site\nYou will redirect to home page in 5 second "); 
                }
                    
                    

                    await next(con);
            
                //await next(con);
            });
            
            app.MapControllers();

            app.Run();
        }
    }
}
