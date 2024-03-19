namespace ReferenceDatabase;

[PrimaryKey(nameof(AccountId), nameof(RoleId))]
public class RoleAccount
{
    [ForeignKey(nameof(AccountId))]
    public Guid AccountId { get; set; }

    public Account Account { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Guid RoleId { get; set; }

    public Role Role { get; set; }
}
