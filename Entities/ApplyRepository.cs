namespace Entities;
public class ApplyRepository : IApplyRepository
{
    ThesisBankContext _context;

    public ApplyRepository(ThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, ApplyDTO)> ReadApplied(int studentID, int thesisID)
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

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadApplicationsByTeacherID(int TeacherID)
    {
        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);

        var thesesWithCurrentTeacherID = await _context.Theses
                                                .Where(t => t.Teacher.Id == TeacherID)
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

        var ApplyDTOList = new List<ApplyDTO>();

        foreach (var item in Applications)
        {
            var student = await stud_repo.ReadStudent(item.StudentID);
            var thesis = await thesis_repo.ReadThesis(item.ThesisID);
            var DTO = new ApplyDTO(item.Status, student.Item2, thesis.Item2);
            ApplyDTOList.Add(DTO);
        }
        
        return ApplyDTOList.AsReadOnly();
    }
}

