namespace Entities;
public class ApplyRepository : IApplyRepository
{
    ThesisBankContext _context;

    public ApplyRepository(ThesisBankContext context){
        _context = context;
    }


    public async Task<(Response, ApplyWithIDDTO)> ReadApplied(int StudentID, int ThesisID){

        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);

        
        var appliesThesis = await _context.Applies
                        .Where(s => s.Id == StudentID)
                        .Where(t => t.Id == ThesisID)
                        .FirstOrDefaultAsync();

        if (appliesThesis == null){
            return (Response.NotFound, null);
        }

        var getStudent = await stud_repo.ReadStudent(StudentID);
        var getThesis = await thesis_repo.ReadThesis(ThesisID);

        var appliedThesisDTO = new ApplyWithIDDTO(appliesThesis.Id, appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }
}

