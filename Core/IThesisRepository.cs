namespace Core;

public interface IThesisRepository
{
    public Task<(Response, ThesisDTO)> ReadThesis(int ThesisId);

    public Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll();

    public Task<IReadOnlyCollection<ThesisDTO>> ReadPendingThesis(int StudentID);
}