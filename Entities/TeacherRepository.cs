namespace Entities;
public class TeacherRepository : ITeacherRepository{

    ThesisBankContext _context;

    public TeacherRepository(ThesisBankContext context){
        _context = context;
    }

    public async Task<(Response, TeacherDTO)> ReadTeacher(int TeacherID){


    var Teacher = await _context.Teachers
                                .Where(t => t.Id == TeacherID)
                                .Select(t => new TeacherDTO(t.Id, t.Name, t.Email))
                                .FirstOrDefaultAsync();
    
    if(Teacher == null){
        return (Response.NotFound, Teacher);
    }

    return (Response.Success, Teacher);     
}


    public Response Accept(int ThesisID){
        throw new NotImplementedException();
    }

    public Response Reject(int ThesisID){
        throw new NotImplementedException();
    }

}