namespace ReferenceDatabase;

public class Customer(string name, string phone, DateTime created, Guid profileId)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Created { get; set; } = created;

    [Required]
    public string Name { get; set; } = name;

    [AllowNull]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = phone;

    [AllowNull]
    public DateTime Birthday { get; set; }

    [AllowNull]
    public string Image { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; } = profileId;

    public Profile Profile { get; set; }

    public ICollection<Invoice> Invoices { get; set; }
}
