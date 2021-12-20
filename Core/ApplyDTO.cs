namespace Core;

public record ApplyDTO(Status Status, StudentDTO? Student, ThesisDTO? Thesis);
public record ApplyDTOWithMinalThesis(int Id, Status Status, StudentDTO Student, ThesisDTOMinimal Thesis);
public record ApplyDTOids(int Id, Status Status, int studentID, int thesisID);
public record ApplyPostDTO(Status status, int studentID, ThesisDTO? Thesis);