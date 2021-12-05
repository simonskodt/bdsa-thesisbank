namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }

    public async Task<ThesisDTO> ReadThesis(int ThesisID){


        var Thesis = await _context.Theses
                                   .Where(t => t.Id == ThesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.Name, t.Description, new TeacherDTO(t.Teacher.Id, t.Teacher.Name, t.Teacher.Email)))
                                   .FirstOrDefaultAsync();
        
        // if(Thesis == null){
        //     return (Response.NotFound, Thesis);
        // }

        // return (Response.Success, Thesis);

        return Thesis == null ? null : Thesis;
            
    }

    public async Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll(){

        var Theses = (await _context.Theses
                       .Select(t => new MinimalThesisDTO(t.Id, t.Name, t.Teacher.Name))
                       .ToListAsync())
                       .AsReadOnly();

            return Theses;
    }

    //Maybe this method should be in the StudentRep ? 
    public Task<IReadOnlyCollection<ThesisDTO>> ReadRequested(){
        throw new NotImplementedException();
    }

}