namespace ReferenceFeature;

public class Database
{
    public static void Configure(WebApplicationBuilder builder) =>
        builder
            .Services
            .AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable(nameof(EnvironmentKey.Database)),
                    configs => configs.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                );
            });
}
