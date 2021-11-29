namespace Entities;
public class TeacherRepository : ITeacherRepository
{
    ThesisBankContext _context;
    StudentRepository _studentRepository;
    ThesisRepository _thesisRepository;

    public TeacherRepository(ThesisBankContext context)
    {
        _context = context;
        _studentRepository = new StudentRepository(_context);
        _thesisRepository = new ThesisRepository(_context);
    }

    public async Task<(Response, TeacherDTO)> ReadTeacher(int TeacherID)
    {
        var Teacher = await _context.Teachers
                                    .Where(t => t.Id == TeacherID)
                                    .Select(t => new TeacherDTO(t.Id, t.Name, t.Email))
                                    .FirstOrDefaultAsync();

        if (Teacher == null)
        {
            return (Response.NotFound, Teacher);
        }

        return (Response.Success, Teacher);
    }

    public async Task<(Response, ApplyDTO)> Accept(int StudentID, int ThesisID)
    {
        return await ChangeStatus(StudentID, ThesisID, Status.Accepted);
    }

    public async Task<(Response, ApplyDTO)> Reject(int StudentID, int ThesisID)
    {
        return await ChangeStatus(StudentID, ThesisID, Status.Denied);
    }

    private async Task<(Response, ApplyDTO)> ChangeStatus(int studentID, int thesisID, Status status)
    {
        var appliesThesis = await _context.Applies
                                .Where(s => s.Id == studentID)
                                .Where(t => t.Id == thesisID)
                                .Where(a => a.Status == Status.Pending)
                                .FirstOrDefaultAsync();

        if (appliesThesis == null)
        {
            return (Response.NotFound, null);
        }

        appliesThesis.Status = status;

        await _context.SaveChangesAsync();

        var getStudent = await _studentRepository.ReadStudent(studentID);
        var getThesis = await _thesisRepository.ReadThesis(thesisID);

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadStudentApplication(int teacherID)
    {
        throw new NotImplementedException();
    }
}