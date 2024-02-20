namespace ReferenceDatabase;

public class Google
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Sub { get; set; }

    [Required]
    public string Picture { get; set; }

    [Required]
    public User User { get; set; }

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }
}
