namespace Entities;
public class StudentRepository : IStudentRepository
{
    IThesisBankContext _context;

    public StudentRepository(IThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, StudentDTO?)> ReadStudent(int studentID)
    {
        var student = await _context.Students
                                   .Where(s => s.Id == studentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, student);
    }

    public async Task<(Response, int?)> ReadStudentIDByName(string studentName)
    {
        var student = await _context.Students
                                   .Where(s => s.Name == studentName)
                                   .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, student.Id);
    }


    public async Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID)
    {

        var applies = await _context.Applies
                        .Where(a => a.StudentID == studentID)
                        .Where(a => a.ThesisID == thesisID)
                        .Where(a => a.Status == Status.Accepted)
                        .FirstOrDefaultAsync();

        if (applies == null)
        {
            return (Response.NotFound, null);
        }
        applies.Status = Status.Archived;

        await _context.SaveChangesAsync();

        var studentDTO = await ReadStudent(studentID);

        var thesisDTO = await _context.Theses
                            .Where(t => t.Id == thesisID)
                            .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                            .FirstOrDefaultAsync();

        if (thesisDTO == null)
        {
            return (Response.NotFound, null);
        }

        var apply_dto = new ApplyDTO(applies.Status, studentDTO.Item2, thesisDTO);

        return (Response.Updated, apply_dto);
    }

    public async Task<Response> RemoveAllApplications(int studentID)
    {

        var allPending = await _context.Applies
                        .Where(p => p.Status == Status.Pending || p.Status == Status.Accepted)
                        .Where(p => p.StudentID == studentID)
                        .ToListAsync();

        _context.Applies.RemoveRange(allPending);

        await _context.SaveChangesAsync();
        return Response.Deleted;

    }
}