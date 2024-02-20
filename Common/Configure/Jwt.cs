namespace ReferenceFeature;

public class Jwt
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddAuthentication(configs =>
            {
                configs.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configs.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                configs.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configs =>
            {
                configs.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Issuer)),
                    ValidAudience = Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Audience)),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.JwtKey)))
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder
            .Services
            .AddDataProtection()
            .UseCryptographicAlgorithms(
                new()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                }
            );
    }
}
