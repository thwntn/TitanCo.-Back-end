namespace ReferenceFeature;

public class Json
{
    public static void Configure(WebApplicationBuilder builder) =>
        builder
            .Services
            .AddControllers()
            .AddNewtonsoftJson(
                options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
}
