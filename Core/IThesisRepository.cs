namespace Core;

    public interface IThesisRepository
{
    public (Response, ThesisDTO) ReadThesis(int id);

    public (Response, IReadOnlyCollection<MinimalThesisDTO>) ReadAll();

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(); 
    
} 