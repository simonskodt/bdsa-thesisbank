namespace Core;

    public interface IThesisRepository
{
    public Task<(Response, ThesisDTO)> ReadThesis(int ThesisId);

    public Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll();

    //Maybe this method should be in the StudentRep ? 
    public Task<IReadOnlyCollection<ThesisDTO>> ReadRequested(); 
    
} 