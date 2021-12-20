namespace Core;

public record ThesisDTO(int Id, string Name, string? Description, TeacherDTO Teacher);
public record ThesisDTODetailed(int Id, string Name, string? Excerpt, TeacherDTO Teacher, Status status, int ApplyID);
public record ThesisDTOMinimal(int Id, string Name, string? Excerpt, string TeacherName);

