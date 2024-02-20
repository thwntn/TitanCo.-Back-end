namespace ReferenceFeature;

public class Logging
{
    public static void Configure(WebApplicationBuilder builder) =>
        builder.Services.AddLogging(options => options.AddFilter(nameof(Microsoft), LogLevel.Information));
}
