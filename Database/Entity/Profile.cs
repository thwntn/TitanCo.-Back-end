namespace ReferenceDatabase;

public class Profile
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Avatar { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string CoverPicture { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public UserStatus Status { get; set; }

    [Required]
    public UserType Type { get; set; }

    public Google Google { get; set; }

    public ICollection<Notification> Notifications { get; set; }

    public ICollection<Stogare> Stogares { get; set; }

    public ICollection<Group> Groups { get; set; }

    public ICollection<GroupMember> GroupMembers { get; set; }

    public ICollection<Planning> Plannings { get; set; }

    public ICollection<Spend> Spends { get; set; }

    public ICollection<Note> Notes { get; set; }

    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; }
}
