namespace ReferenceDatabase;

public class Spend
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string DateTime { get; set; }

    [Required]
    public string Amount { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    public User User { get; set; }
}
