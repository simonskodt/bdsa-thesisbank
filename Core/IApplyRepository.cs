namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID);

    public Task<IReadOnlyCollection<ApplyDTO>> ReadApplicationsByTeacherID(int teacherID);
}