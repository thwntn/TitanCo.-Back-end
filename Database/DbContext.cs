namespace ReferenceDatabase;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Account> Account { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<RoleAccount> RoleAccount { get; set; }
    public DbSet<LoginAccount> LoginAccount { get; set; }
    public DbSet<Profile> Profile { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Google> Google { get; set; }
    public DbSet<Stogare> Stogare { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<GroupMember> GroupMember { get; set; }
    public DbSet<Planning> Planning { get; set; }
    public DbSet<Spend> Spend { get; set; }
    public DbSet<Note> Note { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Invoice> Invoice { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<InvoiceProduct> InvoiceProduct { get; set; }
    public DbSet<InvoiceDiscount> InvoiceDiscount { get; set; }
    public DbSet<Discount> Discount { get; set; }
}
