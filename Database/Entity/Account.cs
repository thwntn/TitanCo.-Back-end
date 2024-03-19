namespace ReferenceDatabase;

public class Account(
    string email,
    string hashPassword,
    string userName,
    string code,
    AccountStatus accountStatus,
    AccounType accounType
)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; } = DateTime.Now;

    [Required]
    public string Email { get; set; } = email;

    [Required]
    public string UserName { get; set; } = userName;

    [Required]
    public string HashPassword { get; set; } = hashPassword;

    [Required]
    public AccountStatus AccountStatus { get; set; } = accountStatus;

    [Required]
    public string Code { get; set; } = code;

    [Required]
    public AccounType AccounType { get; set; } = accounType;

    [AllowNull]
    public Guid ParentAccountId { get; set; }

    [Required]
    public Profile Profile { get; set; }

    public ICollection<LoginAccount> LoginAccounts { get; set; }

    public ICollection<RoleAccount> RoleAccounts { get; set; }

    [AllowNull]
    public Google Google { get; set; }

    public string Token;
}
