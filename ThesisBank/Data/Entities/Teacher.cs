namespace Entities;
using System.ComponentModel.DataAnnotations;
public class Teacher
{
    public int ID {get; set;}

    [StringLength(20)]
    public string? name {get; set;}

    [EmailAddress]
    public string? email {get; set;}

    public ICollection<Thesis>? ownedTheses {get; set;} 

}