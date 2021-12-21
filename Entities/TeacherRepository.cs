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
        var teacherDTO = await _context.Teachers
                                    .Where(t => t.Id == TeacherID)
                                    .Select(t => new TeacherDTO(t.Id, t.Name, t.Email))
                                    .FirstOrDefaultAsync();

        if (teacherDTO == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, teacherDTO);
    }

    public async Task<(Response, ApplyDTO?)> Accept(int studentID, int thesisID)
    {
        var apply = await _context.Applies
                                .Where(s => s.StudentID == studentID)
                                .Where(t => t.ThesisID == thesisID)
                                .Where(a => a.Status == Status.Pending)
                                .FirstOrDefaultAsync();

        if (apply == null)
        {
            return (Response.NotFound, null);
        }

        apply.Status = Status.Accepted;

        await _context.SaveChangesAsync();

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

        var appliedThesisDTO = new ApplyDTO(apply.Status, studentDTO, thesisDTO);

        return (Response.Success, appliedThesisDTO);
    
    }

     private async Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>> ReadApplicationsByTeacherID(int teacherID)
    {

        var thesesWithCurrentTeacherID = await _context.Theses
                                                .Where(t => t.Teacher.Id == teacherID)
                                                .ToListAsync();
        var thesesIDs = new List<int>();

        var applies = new List<Apply>();

        foreach (var item in thesesWithCurrentTeacherID)
        {
            thesesIDs.Add(item.Id);
        }

        var applications = await _context.Applies
                                .Where(s => thesesIDs.Contains(s.ThesisID))
                                .ToListAsync();

        var applyDTOList = new List<ApplyDTOWithMinalThesis>();

        foreach (var item in applications)
        {
            var studentDTO = await _context.Students
                                   .Where(s => s.Id == item.StudentID)
                                   .Select(s => new StudentDTO(s.Id, s.Name, s.Email))
                                   .FirstOrDefaultAsync();

            var thesisDTO = await _context.Theses
                                   .Where(t => t.Id == item.ThesisID)
                                   .Select(t => new ThesisDTOMinimal(t.Id, t.Name, t.Excerpt, t.Teacher.Name))
                                   .FirstOrDefaultAsync();

            var DTO = new ApplyDTOWithMinalThesis(item.Id, item.Status, studentDTO, thesisDTO);
            applyDTOList.Add(DTO);
        }

        return applyDTOList.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ApplyDTOWithMinalThesis>?> ReadPendingApplicationsByTeacherID(int teacherID)
    {
        var ownedAppliedEntries = await ReadApplicationsByTeacherID(teacherID);

        var applyDTOList = new List<ApplyDTOWithMinalThesis>();


        if (ownedAppliedEntries == null)
        {
            return applyDTOList;
        }

        foreach (var item in ownedAppliedEntries)
        {
            if (item.Status == Status.Pending)
            {
                applyDTOList.Add(item);
            }
        }

        return applyDTOList.AsReadOnly();
    }
}