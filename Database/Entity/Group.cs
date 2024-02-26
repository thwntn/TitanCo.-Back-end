namespace ReferenceDatabase;

public class Group
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Stogare> DataGroups { get; set; }

    public ICollection<GroupMember> Members { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public string ProfileId { get; set; }

    public Profile Profile { get; set; }
}
