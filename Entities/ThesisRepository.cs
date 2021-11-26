namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public ThesisDTO ReadThesis(int ThesisID){

        var Thesis = from t in _context.Theses
                     where t.Id == ThesisID
                     select new ThesisDTO(t.Id, t.Name, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email));

            return Thesis.FirstOrDefault();
    }

    public IReadOnlyCollection<MinimalThesisDTO> ReadlAll(){
        throw new NotImplementedException();
    }

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(){
        throw new NotImplementedException();
    }

}