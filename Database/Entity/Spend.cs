namespace ReferenceDatabase;

public class Spend
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string DateTime { get; set; }

    [Required]
    public string Amount { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }
}
