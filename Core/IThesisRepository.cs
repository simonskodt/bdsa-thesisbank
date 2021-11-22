namespace Core;

    public interface IThesisRepository
{
    public ThesisDTO ReadThesis(int id);

    public IReadOnlyCollection<ThesisDTO> ReadlAll();

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(); 
    
} 