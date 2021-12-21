namespace Entities;

public class Apply
{
    public int Id { get; set; }
    public Status Status { get; set; }
    public int ThesisID { get; set; }
    public Thesis Thesis { get; set; }
    public int StudentID { get; set; }
    public Student Student { get; set; }
    public Apply(Thesis thesis, Student student)
    {
        Status = Status.Pending;
        Thesis = thesis;
        Student = student;
    }

    //This constructor is only used for testing purpose 
    public Apply(int thesisID, int studentID)
    {
        Status = Status.Pending;
        ThesisID = thesisID;
        StudentID = studentID;
    }
}