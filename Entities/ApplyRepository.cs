namespace Entities;
public class ApplyRepository : IApplyRepository
{
    ThesisBankContext _context;

    public ApplyRepository(ThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID)
    {
        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);


        var appliesThesis = await _context.Applies
                        .Where(s => s.Student.Id == studentID)
                        .Where(t => t.Thesis.Id == thesisID)
                        .FirstOrDefaultAsync();

        if (appliesThesis == null)
        {
            return (Response.NotFound, null);
        }

        var getStudent = await stud_repo.ReadStudent(studentID);
        var getThesis = await thesis_repo.ReadThesis(thesisID);

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }

       public async Task<IReadOnlyCollection<ApplyDTOid>> ReadApplied() =>
        (await _context.Applies
                       .Select(c => new ApplyDTOid(c.Id, c.Status, c.StudentID, c.ThesisID))
                       .ToListAsync())
                       .AsReadOnly();

    public async Task<IReadOnlyCollection<ApplyWithIDDTO>> ReadAppliedByStudentAndStatus(int StudentID)
    {
        var stud_repo = new StudentRepository(_context);

        var studentDTO = await stud_repo.ReadStudent(StudentID);
                            
        var Applications = await _context.Applies
                        .Where(a => a.Status != Status.Archived)
                        .Where(a => a.StudentID == studentDTO.Item2.Id)
                        .Select(a => new MaximalisticDTO(a.ThesisID, a.Thesis.Name, a.Thesis.Excerpt, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email), a.Status, a.Id))
                        .ToListAsync();


        var ApplyDTOs = new List<ApplyWithIDDTO>();


        foreach (var thesis in Applications)
        {
            var DTO = new ApplyWithIDDTO(thesis.ApplyID,thesis.status, studentDTO.Item2, new MinimalThesisDTO(thesis.Id, thesis.Name, thesis.Excerpt, thesis.Teacher.Name));
            ApplyDTOs.Add(DTO);
        }

        return ApplyDTOs.AsReadOnly();

    }

    public async Task<(Response, ApplyDTOid?)> ApplyForThesis(int studentID, int ThesisID)
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
        _context.Applies.Add(entity);
        await _context.SaveChangesAsync();

        return (Response.Created, new ApplyDTOid(entity.Id, entity.Status, studentID, ThesisID));
   }


   public async Task<Response> RemoveRequest(int applyId)
    {
        Apply? pending = await _context.Applies
                        .Where(p => p.Id == applyId)
                        .FirstOrDefaultAsync();

        if (pending == null)
        {
            return Response.NotFound;
        }

        _context.Applies.Remove(pending);

        await _context.SaveChangesAsync();
        return Response.Deleted;
    }

}

