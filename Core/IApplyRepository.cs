namespace Core;

public interface IApplyRepository
{
    public Task<(Response, ApplyWithIDDTO)> ReadApplied(int StudentID, int ThesisID);
    
}