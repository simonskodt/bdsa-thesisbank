namespace Entities;
public class Student
{
    public int Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public ICollection<Apply>? Applies { get; set; }
}