namespace ReferenceFeature;

public class Signalr
{
    public static void Configure(WebApplicationBuilder builder) =>
        builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();
}
