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

    public async Task<(Response, ApplyDTO)> Accept(int StudentID, int ThesisID){
        
        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);


        var appliesThesis = await _context.Applies
                                .Where(s => s.Id == StudentID)
                                .Where(t => t.Id == ThesisID)
                                .Where(a => a.Status == Status.Pending)
                                .FirstOrDefaultAsync();

        if (appliesThesis == null){
            return (Response.NotFound, null);
        }

        appliesThesis.Status = Status.Accepted;

        await _context.SaveChangesAsync();

        var getStudent = await stud_repo.ReadStudent(StudentID);
        var getThesis = await thesis_repo.ReadThesis(ThesisID);

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }

    public async Task<(Response, ApplyDTO)> Reject(int StudentID, int ThesisID){

        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);


        var appliesThesis = await _context.Applies
                                .Where(s => s.Id == StudentID)
                                .Where(t => t.Id == ThesisID)
                                .Where(a => a.Status == Status.Pending)
                                .FirstOrDefaultAsync();

        if (appliesThesis == null){
            return (Response.NotFound, null);
        }

        appliesThesis.Status = Status.Denied;

        await _context.SaveChangesAsync();

        var getStudent = await stud_repo.ReadStudent(StudentID);
        var getThesis = await thesis_repo.ReadThesis(ThesisID);

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadStudentApplication(int TeacherID){

        var Applications = _context.Applies
                                .Where()

    }

}