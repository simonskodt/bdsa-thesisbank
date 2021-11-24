namespace Entities;
public class Student
{
    public int Id { get; set; }

    [StringLength(20)]
    public string? name { get; set; }

    [EmailAddress]
    public string? email { get; set; }
}