namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID);
    public Task<(Response, ApplyDTOid?)> ApplyForThesis(int studentID, int thesisID);
    public Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadAppliedByStudentAndStatus(int StudentID);
    public Task<IReadOnlyCollection<ApplyDTOid>> ReadApplied();
    public Task<Response> RemoveRequest(int applyID);
}