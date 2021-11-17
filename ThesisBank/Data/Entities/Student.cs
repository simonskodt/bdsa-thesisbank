namespace Entities;
using System.ComponentModel.DataAnnotations;
public class Student
{
    public int ID {get; set;}

    [StringLength(20)]
    public string? name {get; set;}

    [EmailAddress]
    public string? email {get; set;}
    public ICollection<Thesis>? appliedTheses {get; set;} 

}