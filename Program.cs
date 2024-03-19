namespace Application;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Reader.Configure();
        Startup.Configure(builder);

        WebApplication app = builder.Build();
        app.MapControllerRoute(string.Empty, string.Empty);

        app.UseCors(nameof(Policy.Cors));
        app.MapHub<MapHub>(string.Empty);

        app.UseSwagger();
        app.UseSwaggerUI(options =>
            options.SwaggerEndpoint(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Swagger)), string.Empty)
        );

        app.UseAuthentication();
        app.UseAuthorization();

        app.Run(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Host)));
    }
}
