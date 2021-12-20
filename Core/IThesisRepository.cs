namespace Core;

public interface IThesisRepository
{
    public Task<(Response, ThesisDTO?)> ReadThesis(int ThesisId);
    public Task<(Response, ApplyDTOids?)> FindApplyDTOid(int StudentID, int ThesisID);
    //Helper method for ReadNonPendingTheses()
    public Task<IReadOnlyCollection<ThesisDTO>> ReadPendingThesis(int StudentID);
    public Task<IReadOnlyCollection<ThesisDTOMinimal>> ReadNonPendingTheses(int studentID);
}