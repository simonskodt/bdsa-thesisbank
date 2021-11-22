namespace Entities;
public class Teacher
{
    public int Id { get; set; }

    [StringLength(20)]
    public string? name { get; set; }

    [EmailAddress]
    public string? email { get; set; }

    public ICollection<Thesis>? ownedTheses { get; set; } 
}