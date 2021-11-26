namespace Entities;
public class Teacher
{
    public int Id { get; set; }

    [StringLength(20)]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public ICollection<Thesis>? OwnedTheses { get; set; } 
}