namespace Core;

public record ThesisDTO(int Id, string Name, string Description, TeacherDTO Teacher);

public record MinimalThesisDTO(int Id, string Name,string Description, string TeacherName);