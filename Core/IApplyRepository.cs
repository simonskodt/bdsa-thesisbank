namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID);
    public Task<IReadOnlyCollection<ApplyDTOids>> ReadApplied();
    public Task<(Response, ApplyDTOids?)> ApplyForThesis(int studentID, int thesisID);
    public Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>?> ReadAppliedByStudentID(int StudentID);
    public Task<Response> DeleteApplied(int applyID);
}