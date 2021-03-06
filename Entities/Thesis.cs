namespace Entities;

public class Thesis
{
    public int Id { get; set; }

    [StringLength(60)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Excerpt { get; set; }
    public Teacher Teacher { get; set; }
    public int TeacherID { get; set; }
    public ICollection<Apply>? Applies { get; set; }
    public Thesis(string name, Teacher teacher)
    {
        Name = name;
        Teacher = teacher;
    }
    //This constructor is only used for testing purpose 
    public Thesis(string name, int teacherID)
    {
        Name = name;
        TeacherID = teacherID;
    }
}