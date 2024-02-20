namespace ReferenceFeature;

public class Cors
{
    public static void Configure(WebApplicationBuilder builder) =>
        builder
            .Services
            .AddCors(
                setup =>
                    setup.AddPolicy(
                        nameof(Policy.Cors),
                        policy =>
                            policy
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .SetIsOriginAllowed(origin => true)
                                .AllowCredentials()
                    )
            );
}
