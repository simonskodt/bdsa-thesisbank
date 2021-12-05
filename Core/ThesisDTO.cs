
namespace Core;

public record ThesisDTO(int Id, string Name, string Description, TeacherDTO Teacher); //Mangler description

public record MinimalThesisDTO(int ID, string Name, string TeacherName);