namespace Entities;
public class TeacherRepository : ITeacherRepository
{

    ThesisBankContext _context;

    public TeacherRepository(ThesisBankContext context){
        _context = context;
    }

    public async Task<(Response, TeacherDTO)> ReadTeacher(int TeacherID)
    {
    var Teacher = await _context.Teachers
                                .Where(t => t.Id == TeacherID)
                                .Select(t => new TeacherDTO(t.Id, t.Name, t.Email))
                                .FirstOrDefaultAsync();
    
    if(Teacher == null){
        return (Response.NotFound, Teacher);
    }

    return (Response.Success, Teacher);     
}

    public async Task<Response, ApplyDTO> Accept(int studentID, int thesisID)
    {
        var student = await _context.Students
                        .Where(s => s.Id == studentID)
                        .FirstOrDefaultAsync();
        
        var thesis = await _context.Theses
                        .Where(t => t.Id == thesisID)
                        .FirstOrDefaultAsync();

        thesis
    }

    public async Task<Response> Reject(int StudentID, int ThesisID)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadStudentApplication(int TeacherID)
    {
            throw new NotImplementedException();
    }

}