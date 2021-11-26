namespace Entities;

public class ThesisRepository : IThesisRepository{


    ThesisBankContext _context;

    public ThesisRepository(ThesisBankContext context){
        _context = context;
    }



    public async Task<(Response, ThesisDTO)> ReadThesis(int ThesisID){

        // var Thesis = await from t in _context.Theses
        //              where t.Id == ThesisID
        //              select new ThesisDTO(t.Id, t.name, new TeacherDTO(t.teacher.Id, t.teacher.name, t.teacher.email));
        
        // if(Thesis.FirstOrDefault() != null){
        //         return (Response.Success, Thesis.FirstOrDefaultAsync());
        // } 
        // return (Response.NotFound, null);

        var Thesis = await _context.Theses
                                   .Where(t => t.Id == ThesisID)
                                   .Select(t => new ThesisDTO(t.Id, t.name, new TeacherDTO(t.teacher.Id, t.teacher.name, t.teacher.email)))
                                   .FirstOrDefaultAsync();
        
        if(Thesis == null){
            return (Response.NotFound, Thesis);
        }

        return (Response.Success, Thesis);
            
    }

    public async Task<IReadOnlyCollection<MinimalThesisDTO>> ReadAll(){

        var Theses = (await _context.Theses
                       .Select(t => new MinimalThesisDTO(t.Id, t.name, t.teacher.name))
                       .ToListAsync())
                       .AsReadOnly();

            return Theses;
    }

    //Maybe this method should be in the StudentRep ? 
    public Task<IReadOnlyCollection<ThesisDTO>> ReadRequested(){
        throw new NotImplementedException();
    }

}