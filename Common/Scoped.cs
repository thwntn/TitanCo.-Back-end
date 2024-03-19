namespace ReferenceFeature;

public class Scoped
{
    public static void Injection(IServiceCollection services)
    {
        // services.AddScoped<IWSManager, WSManager>();
        services.AddScoped<IWSConnection, WSConnection>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IAuth, AuthService>();
        services.AddScoped<IJwt, JwtService>();
        services.AddScoped<IProfile, ProfileService>();
        services.AddScoped<IExtra, ExtraService>();
        services.AddScoped<IRole, RoleService>();
        services.AddScoped<IMail, MailService>();
        services.AddScoped<IGoogle, GoogleService>();
        services.AddScoped<INotification, NotificationService>();
        services.AddScoped<IGemini, GeminiService>();

        services.AddScoped<IStogare, StogareService>();
        services.AddScoped<IShare, ShareService>();
        services.AddScoped<IGroup, GroupService>();
        services.AddScoped<ITrash, TrashService>();
        services.AddScoped<IPlanning, PlanningService>();
        services.AddScoped<ISpend, SpendService>();
        services.AddScoped<INote, NoteService>();
        services.AddScoped<IInvoice, InvoiceService>();
        services.AddScoped<IProduct, ProductService>();
        services.AddScoped<IPayment, PaymentService>();
        services.AddScoped<ICustomer, CustomerService>();
        services.AddScoped<IDiscount, DiscountService>();
    }
}
