namespace Entities;

public class ThesisRepository : IThesisRepository
{
    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, ThesisDTO?)> ReadThesis(int ThesisID)
    {
        var thesis = await _context.Theses
                                   .Where(t => t.Id == ThesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                                   .FirstOrDefaultAsync();

        if (thesis == null)
        {
            return (Response.NotFound, null);
        }

        return (Response.Success, thesis);
    }

    public async Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll()
    {
        var theses = (await _context.Theses
                       .Select(t => new MinimalThesisDTO(t.Id, t.Name, t.Description, t.Teacher.Name))
                       .ToListAsync())
                       .AsReadOnly();

        return theses;
    }

    public async Task<IReadOnlyCollection<ThesisDTO>> ReadPendingThesis(int studentID)
    {
        var thesesID = (await _context.Applies
                       .Where(a => a.StudentID == studentID)
                       .Where(a => a.Status == Status.Pending)
                       .Select(a => new ThesisDTO(a.ThesisID, a.Thesis.Name, a.Thesis.Description, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email)))
                       .ToListAsync())
                       .AsReadOnly();

        return thesesID;
    }

    public async Task<(Response, ApplyDTOid)> FindApplyDTOID(int StudentID, int ThesisID){
        Apply? pending = await _context.Applies
                        .Where(p => p.StudentID == StudentID)
                        .Where(p => p.ThesisID == ThesisID)
                        .FirstOrDefaultAsync();

        if (pending == null)
        {
            return (Response.NotFound, null);
        }

        var DTO = new ApplyDTOid(pending.Id, pending.Status, pending.StudentID, pending.ThesisID);

        return (Response.Success, DTO);
    }

        public async Task<IReadOnlyCollection<MinimalThesisDTO>> ReadNonPendingTheses(int studentID){
            var appliedThesisList = await ReadPendingThesis(studentID);

            var appliedThesisListIDs = new List<int>();

            foreach (var thesis in appliedThesisList)
            {
                Console.WriteLine("THESIS:" + thesis);
                appliedThesisListIDs.Add(thesis.Id);
            }


            var allThesis = (await _context.Theses
                       .Select(t => new MinimalThesisDTO(t.Id, t.Name, t.Description, t.Teacher.Name))
                       .ToListAsync());


            var returnList = new List<MinimalThesisDTO>();


            foreach (var thesis in allThesis)
            {   
                if (appliedThesisListIDs.Contains(thesis.Id) == false){
                    returnList.Add(thesis);
                    Console.WriteLine("APPENDING: " + thesis);

                }
            }
            return returnList.AsReadOnly();

        }


}