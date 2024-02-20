namespace ReferenceDatabase;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    public NotificationType Type { get; set; }

    [Required]
    public string JsonData { get; set; }

    [Required]
    public bool IsRead { get; set; }

    [Required]
    public bool Handle { get; set; }

    [ForeignKey(nameof(FromId))]
    public int FromId { get; set; }

    [NotMapped]
    public User From { get; set; }

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    public User User { get; set; }
}
