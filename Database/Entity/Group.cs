namespace ReferenceDatabase;

public class Group
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Stogare> DataGroups { get; set; }

    public ICollection<GroupMember> Members { get; set; }

    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    public User User { get; set; }
}
