namespace Core;


public record ApplyDTO (Status Status, StudentDTO Student, ThesisDTO Thesis);

public record ApplyCreateDTO(StudentDTO StudentID, ThesisDTO ThesisID);
public record ApplyDTOTemp

{
    public int Id { get; set; }
    public Status Status { get; set; }
    public StudentDTO Student { get; set; }

    public ThesisDTO Thesis { get; set; }
}