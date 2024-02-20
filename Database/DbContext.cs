namespace ReferenceDatabase;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Google> Google { get; set; }
    public DbSet<Stogare> Stogare { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<GroupMember> GroupMember { get; set; }
    public DbSet<Planning> Planning { get; set; }
    public DbSet<Spend> Spend { get; set; }
    public DbSet<Note> Note { get; set; }
}
