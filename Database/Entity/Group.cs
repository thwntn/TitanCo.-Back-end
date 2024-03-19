namespace ReferenceDatabase;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Image { get; set; }

    public ICollection<Stogare> Stogares { get; set; }

    public ICollection<GroupMember> Members { get; set; }

    [ForeignKey(nameof(ProfileId))]
    public Guid ProfileId { get; set; }

    public Profile Profile { get; set; }
}
