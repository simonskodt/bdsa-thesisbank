namespace Entities;
using System.ComponentModel.DataAnnotations;
public class Thesis
{
    public int ID {get; set;}

    [StringLength(20)]
    public string? name {get; set;}

    
    public string? description {get; set;}

    public int ownerTID {get; set;}

    public ICollection<Student>? appliedStudents {get; set;}

}