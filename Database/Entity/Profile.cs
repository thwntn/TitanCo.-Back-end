namespace ReferenceDatabase;

public class Profile(string name, string email)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = name;

    [Required]
    public string Avatar { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = email;

    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string CoverPicture { get; set; } = string.Empty;

    [AllowNull]
    public string Address { get; set; }

    [ForeignKey(nameof(AccountId))]
    public Guid AccountId { get; set; }

    public ICollection<Notification> Notifications { get; set; }

    public ICollection<Stogare> Stogares { get; set; }

    public ICollection<Group> Groups { get; set; }

    public ICollection<GroupMember> GroupMembers { get; set; }

    public ICollection<Planning> Plannings { get; set; }

    public ICollection<Spend> Spends { get; set; }

    public ICollection<Note> Notes { get; set; }

    public ICollection<Customer> Customers { get; set; }

    public ICollection<Payment> Payments { get; set; }
}
