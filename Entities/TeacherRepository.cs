namespace Entities;

public class TeacherRepository : ITeacherRepository
{
    IThesisBankContext _context;

    public TeacherRepository(IThesisBankContext context)
    {
        _context = context;
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
        var appliesThesis = await _context.Applies
                                .Where(s => s.StudentID == studentID)
                                .Where(t => t.ThesisID == thesisID)
                                .Where(a => a.Status == Status.Pending)
                                .FirstOrDefaultAsync();

        if (appliesThesis == null)
        {
            return (Response.NotFound, null);
        }

        appliesThesis.Status = Status.Accepted;

        await _context.SaveChangesAsync();

        var getStudent = await _context.Students
                                   .Where(s => s.Id == studentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

        var getThesis = await _context.Theses
                                   .Where(t => t.Id == thesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                                   .FirstOrDefaultAsync();

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, getStudent, getThesis);

        return (Response.Success, appliedThesisDTO);
    
    }

     public async Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadApplicationsByTeacherID(int teacherID)
    {

        var thesesWithCurrentTeacherID = await _context.Theses
                                                .Where(t => t.Teacher.Id == teacherID)
                                                .ToListAsync();
        var ThesesIDs = new List<int>();

        var Applies = new List<Apply>();

        foreach (var item in thesesWithCurrentTeacherID)
        {
            ThesesIDs.Add(item.Id);
        }

        var Applications = await _context.Applies
                                .Where(s => ThesesIDs.Contains(s.ThesisID))
                                .ToListAsync();

        var ApplyDTOList = new List<ApplyWithIDDTO>();

        foreach (var item in Applications)
        {
            var student = await _context.Students
                                   .Where(s => s.Id == item.StudentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

            var thesis = await _context.Theses
                                   .Where(t => t.Id == item.ThesisID)
                                   .Select(t => new MinimalThesisDTO(t.Id, t.Name, t.Excerpt, t.Teacher.Name))
                                   .FirstOrDefaultAsync();

            var DTO = new ApplyWithIDDTO(item.Id, item.Status, student, thesis);
            ApplyDTOList.Add(DTO);
        }

        return ApplyDTOList.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadPendingStudentApplication(int teacherID)
    {
        var ownedAppliedEntries = await ReadApplicationsByTeacherID(teacherID);

        if (ownedAppliedEntries == null)
        {
            return null;
        }

        var ApplyDTOList = new List<ApplyWithIDDTO>();

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