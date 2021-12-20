namespace Entities;
public class ApplyRepository : IApplyRepository
{
    IThesisBankContext _context;

    public ApplyRepository(IThesisBankContext context)
    {
        _context = context;
    }

    //This method is only implemented for testing purposed - used to test TeacherRepo: Accept();
    public async Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID)
    {

        var appliesThesis = await _context.Applies
                        .Where(s => s.Student.Id == studentID)
                        .Where(t => t.Thesis.Id == thesisID)
                        .FirstOrDefaultAsync();

        if (appliesThesis == null)
        {
            return (Response.NotFound, null);
        }

        var studentDTO = await _context.Students
                        .Where(s => s.Id == studentID)
                        .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                        .FirstOrDefaultAsync();

        if (studentDTO == null)
        {
            return (Response.NotFound, null);
        }

        var thesisDTO = await _context.Theses
                        .Where(t => t.Id == thesisID)
                        .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                        .FirstOrDefaultAsync();

        if (thesisDTO == null)
        {
            return (Response.NotFound, null);
        }

        var appliedThesisDTO = new ApplyDTO(appliesThesis.Status, studentDTO, thesisDTO);

        return (Response.Success, appliedThesisDTO);
    }

    public async Task<IReadOnlyCollection<ApplyDTOids>> ReadApplied() =>
    (await _context.Applies
                    .Select(c => new ApplyDTOids(c.Id, c.Status, c.StudentID, c.ThesisID))
                    .ToListAsync())
                    .AsReadOnly();

    public async Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>?> ReadAppliedByStudentID(int StudentID)
    {
        var studentDTO = await _context.Students
                                   .Where(s => s.Id == StudentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

        if (studentDTO == null)
        {
            return null;
        }
                            
        var Applications = await _context.Applies
                        .Where(a => a.Status != Status.Archived)
                        .Where(a => a.StudentID == studentDTO.Id)
                        .Select(a => new ThesisDTODetailed(a.ThesisID, a.Thesis.Name, a.Thesis.Excerpt, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email), a.Status, a.Id))
                        .ToListAsync();


        var ApplyDTOs = new List<ApplyDTOWithMinalThesis>();


        foreach (var thesis in Applications)
        {
            var DTO = new ApplyDTOWithMinalThesis(thesis.ApplyID,thesis.status, studentDTO, new ThesisDTOMinimal(thesis.Id, thesis.Name, thesis.Excerpt, thesis.Teacher.Name));
            ApplyDTOs.Add(DTO);
        }

        return ApplyDTOs.AsReadOnly();

    }

    public async Task<(Response, ApplyDTOids?)> ApplyForThesis(int studentID, int ThesisID)
    {
        var student = await _context.Students
                           .Where(s => s.Id == studentID)
                           .FirstOrDefaultAsync();

        if (student == null)
        {
            return (Response.NotFound, null);
        }

        var thesis = await _context.Theses
                        .Where(t => t.Id == ThesisID)
                        .FirstOrDefaultAsync();

        if (thesis == null)
        {
            return (Response.NotFound, null);
        }

        var entity = new Apply(thesis, student);
        _context.Applies.Add(entity);
        await _context.SaveChangesAsync();

        return (Response.Created, new ApplyDTOids(entity.Id, entity.Status, studentID, ThesisID));
   }


   public async Task<Response> DeleteApplied(int applyId)
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

