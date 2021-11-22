namespace Core;

public record TeacherDTO(int Id, string Name, string Email);

public record TeacherDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}