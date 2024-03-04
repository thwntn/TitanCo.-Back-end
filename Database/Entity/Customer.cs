using System.Diagnostics.CodeAnalysis;

namespace ReferenceDatabase;

public class Customer(string name, string phone)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime Created { get; set; }

    [Required]
    public string Name { get; set; } = name;

    [AllowNull]
    public string FullName { get; set; } = name;

    [Required]
    public string Phone { get; set; } = phone;

    [AllowNull]
    public DateTime Birthday { get; set; }

    [AllowNull]
    public string Image { get; set; }

    public ICollection<Invoice> Invoices { get; set; }
}
