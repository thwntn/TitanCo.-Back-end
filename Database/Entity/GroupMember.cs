namespace ReferenceDatabase;

[PrimaryKey(nameof(GroupId), nameof(ProfileId))]
public class GroupMember
{
    [ForeignKey(nameof(GroupId))]
    public Guid GroupId { get; set; }

    public Group Group { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public Profile Profile { get; set; }

    [Required]
    public GroupInviteStatus Status { get; set; }
}
