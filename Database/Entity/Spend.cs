namespace ReferenceDatabase;

public class Spend
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string DateTime { get; set; }

    [Required]
    public string Amount { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }
}
