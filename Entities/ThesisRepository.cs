namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public (Response, ThesisDTO) ReadThesis(int ThesisID){

        var Thesis = from t in _context.Theses
                     where t.Id == ThesisID
                     select new ThesisDTO(t.Id, t.name, new TeacherDTO(t.teacher.Id, t.teacher.name, t.teacher.email));
        
        if(Thesis.FirstOrDefault() != null){
                return (Response.Success, Thesis.FirstOrDefault());
        } 
        return (Response.NotFound, null);
            
    }

    public (Response, IReadOnlyCollection<MinimalThesisDTO>) ReadAll(){
        var Theses = from t in _context.Theses
                     select new MinimalThesisDTO(t.Id, t.name, t.teacher.name);

            return (Response.Success, Theses.ToList().AsReadOnly());
    }

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(){
        throw new NotImplementedException();
    }

}