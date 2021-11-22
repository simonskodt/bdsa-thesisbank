namespace Entities;
public class TeacherRepository : ITeacherRepository{

    ThesisBankContext _context;

    public TeacherRepository(ThesisBankContext context){
        _context = context;
    }


    public Response Accept(int ThesisID){
        throw new NotImplementedException();
    }

    public Response Reject(int ThesisID){
        throw new NotImplementedException();
    }

}
