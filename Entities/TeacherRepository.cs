namespace Entities;

public class TeacherRepository : ITeacherRepository
{
    ThesisBankContext _context;
    StudentRepository _studentRepository;
    ThesisRepository _thesisRepository;
    ApplyRepository _ApplyRepository;

    public TeacherRepository(ThesisBankContext context)
    {
        _context = context;
        _studentRepository = new StudentRepository(_context);
        _thesisRepository = new ThesisRepository(_context);
        _ApplyRepository = new ApplyRepository(_context);
    }

    public async Task<(Response, TeacherDTO?)> ReadTeacher(int TeacherID)
    {
        var teacher = await _context.Teachers
                                    .Where(t => t.Id == TeacherID)
                                    .Select(t => new TeacherDTO(t.Id, t.Name, t.Email))
                                    .FirstOrDefaultAsync();

        if (teacher == null)
        {
            return (Response.NotFound, teacher);
        }

        return (Response.Success, teacher);
    }

    public async Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID)
    {
        return await ChangeStatus(studentID, thesisID, Status.Accepted);
    }

    public async Task<(Response, ApplyDTO?)> Reject(int studentID, int thesisID)
    {
        return await ChangeStatus(studentID, thesisID, Status.Denied);
    }

    private async Task<(Response, ApplyDTO?)> ChangeStatus(int studentID, int thesisID, Status status)
    {
        var appliesThesis = await _context.Applies
                                .Where(s => s.StudentID == studentID)
                                .Where(t => t.ThesisID == thesisID)
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

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadPendingStudentApplication(int teacherID)
    {
        var ownedAppliedEntries = await _ApplyRepository.ReadApplicationsByTeacherID(teacherID);

        if (ownedAppliedEntries == null)
        {
            return null;
        }

        var ApplyDTOList = new List<ApplyDTO>();

        foreach (var item in ownedAppliedEntries)
        {
            if (item.Status == Status.Pending)
            {
                ApplyDTOList.Add(item);
            }
        }

        return ApplyDTOList.AsReadOnly();
    }
}