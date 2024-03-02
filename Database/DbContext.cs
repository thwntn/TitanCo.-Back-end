namespace ReferenceDatabase;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : IdentityDbContext(options)
{
    public DbSet<Profile> Profile { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Google> Google { get; set; }
    public DbSet<Stogare> Stogare { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<GroupMember> GroupMember { get; set; }
    public DbSet<Planning> Planning { get; set; }
    public DbSet<Spend> Spend { get; set; }
    public DbSet<Note> Note { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<InvoiceProduct> InvoiceProduct { get; set; }
}
