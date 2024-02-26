namespace ReferenceDatabase;

public class Google
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Sub { get; set; }

    [Required]
    public string Picture { get; set; }

    [Required]
    public Profile Profile { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }
}
