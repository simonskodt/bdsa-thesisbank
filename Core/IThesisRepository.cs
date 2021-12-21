namespace Core;

public interface IThesisRepository
{
    public Task<(Response, ThesisDTO?)> ReadThesis(int ThesisId);
    public Task<(Response, ApplyDTOids?)> FindApplyDTOid(int StudentID, int ThesisID);
    public Task<IReadOnlyCollection<ThesisDTOMinimal>> ReadNonPendingTheses(int studentID);
}