namespace Entities;

public class Apply
{
    public int Id { get; set; }
    public Status Status {get; set;}

    public int ThesisID { get; set; }
    public Thesis Thesis { get; set; } 
        
    public int StudentID {get; set;}
    public Student Student {get; set;}

}