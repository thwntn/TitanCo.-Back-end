namespace ReferenceDatabase;

public class Stogare
{
    [Key]
    public string Id { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string MapName { get; set; }

    [Required]
    public string Parent { get; set; }

    [Required]
    public string Url { get; set; }

    public string Thumbnail { get; set; }

    [Required]
    public StogareType Type { get; set; }

    [Required]
    public StogareStatus Status { get; set; }

    [Required]
    public long Size { get; set; }

    [ForeignKey(nameof(GroupId))]
    public string? GroupId { get; set; }

    public Group Group { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }
}
