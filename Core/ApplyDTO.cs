namespace Core;

public record ApplyDTO(Status Status, StudentDTO? Student, ThesisDTO Thesis);

public record ApplyWithIDDTO(int Id, Status Status, StudentDTO Student, ThesisDTO Thesis);

public record ApplyMinimalDTO(int id, ThesisDTO thesis);

public record ApplyCreateDTO(StudentDTO StudentID, ThesisDTO ThesisID);