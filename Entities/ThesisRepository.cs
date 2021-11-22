namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public ThesisDTO ReadThesis(int ThesisID){

        var Thesis = from t in _context.Theses
        where t.Id == ThesisID
        select new ThesisDTO(t.Id, t.name, new TeacherDTO(t.teacher.Id, t.teacher.name, t.teacher.email)); //Should use some create method from Teacher?? 

            return Thesis.FirstOrDefault();
    }

    public IReadOnlyCollection<ThesisDTO> ReadlAll(){
        throw new NotImplementedException();
    }

    //Maybe this method should be in the StudentRep ? 
    public IReadOnlyCollection<ThesisDTO> ReadRequested(){
        throw new NotImplementedException();
    }

}