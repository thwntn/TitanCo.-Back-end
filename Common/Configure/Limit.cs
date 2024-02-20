namespace ReferenceFeature;

public class Limit
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
        builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = long.MaxValue);
    }
}
