using System.ComponentModel.DataAnnotations;

namespace Entities;
public class Student
{
    public int ID {get; set;}

    [StringLength(20)]
    public string? name {get; set;}

    [EmailAddress]
    public string? email {get; set;}
    public ICollection<Thesis>? appliedTheses {get; set;} 

}