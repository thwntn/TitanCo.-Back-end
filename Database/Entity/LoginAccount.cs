namespace ReferenceDatabase;

public class LoginAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [ForeignKey(nameof(AccountId))]
    public Guid AccountId { get; set; }

    [AllowNull]
    public Account Account { get; set; }
}
