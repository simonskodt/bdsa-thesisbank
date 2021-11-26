namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public async Task<(Response, ThesisDTO)> ReadThesis(int ThesisID){


        var Thesis = await _context.Theses
                                   .Where(t => t.Id == ThesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                                   .FirstOrDefaultAsync();
        
        if(Thesis == null){
            return (Response.NotFound, Thesis);
        }

        return (Response.Success, Thesis);
            
    }

    public async Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll(){

        var Theses = (await _context.Theses
                       .Select(t => new MinimalThesisDTO(t.Id, t.Name, t.Description, t.Teacher.Name))
                       .ToListAsync())
                       .AsReadOnly();

            return Theses;
    }

    public async Task<IReadOnlyCollection<ThesisDTO>> ReadPendingThesis(int StudentID){
        var ThesesID = (await _context.Applies
                       .Where(a => a.StudentID == StudentID)
                       .Select(a => new ThesisDTO(a.ThesisID, a.Thesis.Name, a.Thesis.Description, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email)))
                       .ToListAsync())
                       .AsReadOnly();

        // var Theses = (await _context.Theses
        //                .Where(t => t.Id IN ThesesID)
        //                .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
        //                 .ToListAsync())
        //                .AsReadOnly();
        
        return ThesesID;
        
    }

}