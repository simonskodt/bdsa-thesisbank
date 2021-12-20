namespace Entities;

public class ThesisRepository : IThesisRepository
{
    IThesisBankContext _context;

    public ThesisRepository(IThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, ThesisDTO?)> ReadThesis(int ThesisID)
    {
        var thesisDTO = await _context.Theses
                                   .Where(t => t.Id == ThesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                                   .FirstOrDefaultAsync();

        if (thesisDTO == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, thesisDTO);
    }

    public async Task<IReadOnlyCollection<ThesisDTO>> ReadPendingThesis(int studentID)
    {
        var thesesIDs = (await _context.Applies
                        .Where(a => a.StudentID == studentID)
                        .Select(a => new ThesisDTO(a.ThesisID, a.Thesis.Name, a.Thesis.Description, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email)))
                        .ToListAsync())
                        .AsReadOnly();

        return thesesIDs;
    }

    //This method is used to find the id of the apply entity that will be deleted.
    public async Task<(Response, ApplyDTOids)> FindApplyDTOid(int StudentID, int ThesisID){
        var pending = await _context.Applies
                        .Where(p => p.StudentID == StudentID)
                        .Where(p => p.ThesisID == ThesisID)
                        .FirstOrDefaultAsync();

        if (pending == null)
        {
            return (Response.NotFound, null);
        }

        var DTO = new ApplyDTOids(pending.Id, pending.Status, pending.StudentID, pending.ThesisID);

        return (Response.Success, DTO);
    }

    public async Task<IReadOnlyCollection<ThesisDTOMinimal>> ReadNonPendingTheses(int studentID){
        var appliedThesisList = await ReadPendingThesis(studentID);

        var appliedThesisListIDs = new List<int>();

        foreach (var thesis in appliedThesisList)
        {
            appliedThesisListIDs.Add(thesis.Id);
        }

        var allTheses = (await _context.Theses
                    .Select(t => new ThesisDTOMinimal(t.Id, t.Name, t.Excerpt, t.Teacher.Name))
                    .ToListAsync());


        var returnList = new List<ThesisDTOMinimal>();

        foreach (var thesis in allTheses)
        {   
            if (appliedThesisListIDs.Contains(thesis.Id) == false){
                returnList.Add(thesis);

            }
        }

        return returnList.AsReadOnly();
        
    }
}