namespace Entities;
public class StudentRepository : IStudentRepository
{
    ThesisBankContext _context;

    public StudentRepository(ThesisBankContext context)
    {
        _context = context;
    }


    public async Task<(Response, StudentDTO)> ReadStudent(int StudentID)
    {
        var student = await _context.Students
                                   .Where(s => s.Id == StudentID)
                                   .Select(s => new StudentDTO { Id = s.Id, Name = s.Name, Email = s.Email })
                                   .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, student);
        }

        return (Response.Success, student);
    }

    public async Task<(Response, ApplyDTO)> ApplyForThesis(int studentID, int ThesisID)
    {
        var student = _context.Students
                        .Where(s => s.Id == studentID)
                        .FirstOrDefault();

        var thesis = _context.Theses
                        .Where(t => t.Id == ThesisID)
                        .FirstOrDefault();


        var entity = new Apply
        {
            Status = Status.Pending,
            Student = student,
            Thesis = thesis
        };

        var StudentDTO = new StudentDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email
        };


        var ThesisDTO = new ThesisDTO(
            thesis.Id,
            thesis.Name,
            thesis.Description,
            new TeacherDTO(thesis.Teacher.Id, thesis.Teacher.Name, thesis.Teacher.Email)
        );

        _context.Applies.Add(entity);

        await _context.SaveChangesAsync();

        return (Response.Success, new ApplyDTO(entity.Status, StudentDTO, ThesisDTO));
    }

    public async Task<(Response, ApplyDTO)> Accept(int ThesisID, int StudentID)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> RemoveAllPendings(int StudentID)
    {
        throw new NotImplementedException();
    }

    public async Task<(Response, ThesisDTO)> RemoveRequest(int ThesisID, int StudentID)
    {
        throw new NotImplementedException();
    }
}