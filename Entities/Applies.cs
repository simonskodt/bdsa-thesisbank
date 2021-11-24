namespace Entities;

public class Applies
{
    public Status status {get; set;}

    [NotMapped]
    public Thesis appliedTheses { get; set; } 

    [NotMapped]
    public Student appliedStudents {get; set;}

}