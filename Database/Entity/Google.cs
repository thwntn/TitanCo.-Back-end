namespace ReferenceDatabase;

public class Google
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required]
    public string Sub { get; set; }

    [Required]
    public string Picture { get; set; }

    [ForeignKey(nameof(accountId))]
    public string accountId { get; set; }
}
