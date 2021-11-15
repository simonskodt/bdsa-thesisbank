namespace ThesisBank.Data
{
    public interface IThesisRepo
    {
        ThesisDTO ReadThesis(int id);
        
        IReadOnlyCollection<ThesisDTO> ReadAll();
        IReadOnlyCollection<ThesisDTO> ReadRequested(IReadOnlyCollection<ThesisDTO> );
    }
}
