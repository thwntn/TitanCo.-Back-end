namespace ReferenceFeature;

public class Startup
{
    public static void Configure(WebApplicationBuilder builder)
    {
        Scoped.Injection(builder.Services);
        Cors.Configure(builder);
        Database.Configure(builder);
        Filter.Configure(builder);
        Json.Configure(builder);
        Jwt.Configure(builder);
        Limit.Configure(builder);
        Logging.Configure(builder);
        Signalr.Configure(builder);
        CronJob.Configure(builder);
    }
}
