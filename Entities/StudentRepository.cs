namespace Entities;
public class StudentRepository : IStudentRepository
{
    ThesisBankContext _context;
    ThesisRepository _repo_Thesis;

    public StudentRepository(ThesisBankContext context)
    {
        _context = context;
        _repo_Thesis = new ThesisRepository(_context);
    }

    public async Task<(Response, StudentDTO?)> ReadStudent(int studentID)
    {
        var student = await _context.Students
                                   .Where(s => s.Id == studentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, student);
        }

        return (Response.Success, student);
    }
    
    public async Task<(Response, int?)> ReadStudentIDByName(string studentName){
        var student = await _context.Students
                                   .Where(s => s.Name == studentName)
                                   .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, student.Id);
    }

    public async Task<(Response, ApplyDTO?)> ApplyForThesis(int studentID, int ThesisID)
    {
        var student = await _context.Students
                           .Where(s => s.Id == studentID)
                           .FirstOrDefaultAsync();

        var thesis = await _context.Theses
                        .Where(t => t.Id == ThesisID)
                        .FirstOrDefaultAsync();


        if (student == null || thesis == null)
        {
            return (Response.NotFound, null);
        }

        var entity = new Apply(thesis, student);

        var StudentDTO = new StudentDTO(student.Id, student.Name, student.Email);

        var ThesisDTO = new ThesisDTO(thesis.Id, thesis.Name, thesis.Description,
                                    new TeacherDTO(thesis.Teacher.Id, thesis.Teacher.Name, thesis.Teacher.Email)
        );

        _context.Applies.Add(entity);

        await _context.SaveChangesAsync();

        return (Response.Success, new ApplyDTO(entity.Status, StudentDTO, ThesisDTO));
    }

    public async Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID)
    {
        Console.WriteLine("ACCEPT METODE BEGYNDER --  ");

        var applies = await _context.Applies
                        .Where(a => a.StudentID == studentID)
                        .Where(a => a.ThesisID == thesisID)
                        .Where(a => a.Status == Status.Accepted)
                        .FirstOrDefaultAsync();

        Console.WriteLine("ACCEPT METODE 1 --  ");

        if (applies == null)
        {
            return (Response.NotFound, null);
        }
        Console.WriteLine("ACCEPT METODE 2 --  ");
        applies.Status = Status.Archived;
        Console.WriteLine("ACCEPT METODE 3 --  ");

        await _context.SaveChangesAsync();

        var resp_stud = await ReadStudent(studentID);
        Console.WriteLine("ACCEPT METODE 4 --  ");
        var resp_thes = await _repo_Thesis.ReadThesis(thesisID);
         Console.WriteLine("ACCEPT METODE 5 --  ");

        var apply_dto = new ApplyDTO(applies.Status, resp_stud.Item2, resp_thes.Item2);
        Console.WriteLine("STATUSEN PÅ VORES TRYKKEDE THESIS ER   --  " + applies.Status);

        return (Response.Updated, apply_dto);
        
    }

    public async Task<Response> RemoveAllPendings(int studentID)
    {
        var allPending = await _context.Applies
                        .Where(p => p.StudentID == studentID)
                        .Where(p => p.Status == Status.Pending)
                        .ToListAsync();

        _context.Applies.RemoveRange(allPending);

        await _context.SaveChangesAsync();
        return Response.Deleted;
    }
}