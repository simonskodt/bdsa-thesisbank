namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID);
    public Task<IReadOnlyCollection<ApplyDTO>> ReadApplicationsByTeacherID(int teacherID);
    public Task<(Response, ApplyDTOid?)> ApplyForThesis(int studentID, int thesisID);
    public Task<IReadOnlyCollection<ApplyDTO>> ReadAppliedByStudentAndStatus(int StudentID);


}