namespace Entities;
public class StudentRepository : IStudentRepository
{
    ThesisBankContext _context;

    public StudentRepository(ThesisBankContext context){
        _context = context;
    }

    public Response Apply(int ThesisID){
        throw new NotImplementedException();
    }

    public Response Accept(int ThesisID){
        throw new NotImplementedException();
    }


    public void RemoveAllPendings(){
        throw new NotImplementedException();
    }

    public Response RemoveRequest(int ThesisID){
        throw new NotImplementedException();
    }
}