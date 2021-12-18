namespace Core;

public record ThesisDTO(int Id, string Name, string? Description, TeacherDTO Teacher);

//NEED TO REFACTORE OR SOMETHING!!!!!
public record ThesisWithStatusDTO(int Id, string Name, string? Description, TeacherDTO Teacher, Status status, int ApplyID);
public record MinimalThesisDTO(int Id, string Name, string? Description, string TeacherName);

