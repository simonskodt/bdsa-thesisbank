
namespace Core;

public record ThesisDTO(int Id, string Name, TeacherDTO Teacher); //Mangler description

public record MinimalThesisDTO
{
    [Required]
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; }

    public TeacherDTO Teacher { get; set; }

}