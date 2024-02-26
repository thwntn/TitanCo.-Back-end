namespace ReferenceFeature;

public class Startup
{
    public static void Configure(WebApplicationBuilder builder)
    {
        // @Cors
        builder.Services.AddCors(setup =>
            setup.AddPolicy(
                nameof(Policy.Cors),
                policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials()
            )
        );

        // @Database Context
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Database)),
                configs => configs.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        });

        // @Filter Exception
        builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

        // @Json
        builder
            .Services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

        // @Jwt
        builder
            .Services.AddAuthentication(configs =>
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
            .Services.AddDataProtection()
            .UseCryptographicAlgorithms(
                new()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                }
            );

        // @Config Transfrorm
        builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
        builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = long.MaxValue);

        // @Logger
        builder.Services.AddLogging(options => options.AddFilter(nameof(Microsoft), LogLevel.Information));

        // @Signalr
        builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();

        // @Roles & Manager User
        builder
            .Services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();

        Scoped.Injection(builder.Services);
        CronJob.Configure(builder);
    }
}
