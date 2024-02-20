namespace ReferenceDatabase;

[PrimaryKey(nameof(GroupId), nameof(UserId))]
public class GroupMember
{
    [ForeignKey(nameof(GroupId))]
    public int GroupId { get; set; }

    public Group Group { get; set; }

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.Restrict)]
    public User User { get; set; }

    [Required]
    public GroupInviteStatus Status { get; set; }
}
