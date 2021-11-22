namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public ThesisDTO ReadThesis(int id){
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<ThesisDTO> ReadlAll(){
        throw new NotImplementedException();
    }

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(){
        throw new NotImplementedException();
    }

}