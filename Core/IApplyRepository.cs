namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyDTO)> ReadApplied(int StudentID, int ThesisID);

    public Task<IReadOnlyCollection<ApplyDTO>> ReadApplicationsByTeacherID(int TeacherID);
    
}