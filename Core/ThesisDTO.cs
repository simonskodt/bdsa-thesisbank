namespace Core;

public record ThesisDTO(int Id, string Name, string? Description, TeacherDTO Teacher);

//NEED TO REFACTORE OR SOMETHING!!!!!
public record MaximalisticDTO(int Id, string Name, string? excerpt, TeacherDTO Teacher, Status status, int ApplyID);
public record MinimalThesisDTO(int Id, string Name, string? excerpt, string TeacherName);

