namespace ReferenceFeature;

public class Startup
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(setup =>
            setup.AddPolicy(
                nameof(Policy.Cors),
                policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials()
            )
        );

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Database)),
                configs => configs.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        });

        builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

        builder
            .Services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

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

        builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue);
        builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = long.MaxValue);

        builder.Services.AddLogging(options => options.AddFilter(nameof(Microsoft), LogLevel.Warning));

        builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(Environment.GetEnvironmentVariable(nameof(EnvironmentKey.VersionSwagger)), new());
            options.AddSecurityDefinition(
                Environment.GetEnvironmentVariable(nameof(EnvironmentKey.TokenScheme)),
                new()
            );
            options.CustomSchemaIds(scheme =>
                scheme.FullName.Replace(
                    Environment.GetEnvironmentVariable(nameof(EnvironmentKey.SwaggerNameReplace)),
                    Environment.GetEnvironmentVariable(nameof(EnvironmentKey.SwaggerNameTo))
                )
            );
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        Scoped.Injection(builder.Services);
        CronJob.Configure(builder);
    }
}
