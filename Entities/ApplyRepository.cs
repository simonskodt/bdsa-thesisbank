namespace Entities;
public class ApplyRepository : IApplyRepository
{
    ThesisBankContext _context;

    public ApplyRepository(ThesisBankContext context){
        _context = context;
    }

    public async Task<(Response, ApplyWithIDDTO)> ReadApplied(int studentID, int thesisID){

        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);

        
        var appliesThesis = await _context.Applies
                        .Where(s => s.Id == studentID)
                        .Where(t => t.Id == thesisID)
                        .FirstOrDefaultAsync();

        if (appliesThesis == null){
            return (Response.NotFound, null);
        }

        var getStudent = await stud_repo.ReadStudent(studentID);
        var getThesis = await thesis_repo.ReadThesis(thesisID);

        var appliedThesisDTO = new ApplyWithIDDTO(appliesThesis.Id, appliesThesis.Status, getStudent.Item2, getThesis.Item2);

        return (Response.Success, appliedThesisDTO);
    }
}

