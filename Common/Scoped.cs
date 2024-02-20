namespace ReferenceFeature;

public class Scoped
{
    public static void Injection(IServiceCollection services)
    {
        // @Realtime
        // services.AddScoped<IWSManager, WSManager>();
        services.AddScoped<IWSConnection, WSConnection>();

        // @Core
        services.AddScoped<IAuth, AuthService>();
        services.AddScoped<ISecurity, SecurityService>();
        services.AddScoped<IUser, UserService>();
        services.AddScoped<INotification, NotificationService>();
        services.AddScoped<IMail, MailService>();
        services.AddScoped<IGoogle, GoogleService>();
        services.AddScoped<IGemini, GeminiService>();

        // @Services
        services.AddScoped<IStogare, StogareService>();
        services.AddScoped<IShare, ShareService>();
        services.AddScoped<IGroup, GroupService>();
        services.AddScoped<ITrash, TrashService>();
        services.AddScoped<IPlanning, PlanningService>();
        services.AddScoped<ISpend, SpendService>();
        services.AddScoped<INote, NoteService>();
    }
}
