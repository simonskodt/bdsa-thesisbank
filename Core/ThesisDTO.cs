namespace Core;

public record ThesisDTO(int Id, string Name, string? Description, TeacherDTO Teacher);
public record MaximalisticDTO(int Id, string Name, string? Excerpt, TeacherDTO Teacher, Status status, int ApplyID);
public record MinimalThesisDTO(int Id, string Name, string? Excerpt, string TeacherName);

