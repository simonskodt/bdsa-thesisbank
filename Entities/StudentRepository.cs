namespace Entities;
public class StudentRepository : IStudentRepository
{
    ThesisBankContext _context;

    public StudentRepository(ThesisBankContext context){
        _context = context;
    }


    public async Task<(Response, StudentDTO)> ReadStudent(int StudentID){

        var Student = await _context.Students
                                   .Where(s => s.Id == StudentID)
                                   .Select(s => new StudentDTO{Id = s.Id, Name = s.Name, Email = s.Email})
                                   .FirstOrDefaultAsync();
        
        if(Student == null){
            return (Response.NotFound, Student);
        }

        return (Response.Success, Student);     
    }

    public async Task<(Response, ApplyDTO)> ApplyForThesis(int studentID, int ThesisID) {

            var student = _context.Students
                            .Where(s => s.Id == studentID)
                            .FirstOrDefault();

            var thesis = _context.Theses
                            .Where(t => t.Id == ThesisID)
                            .FirstOrDefault();


            var entity = new Apply{
                Status = Status.Pending,
                Student = student,
                Thesis = thesis
            };

            var StudentDTO = new StudentDTO{
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };


            var ThesisDTO = new ThesisDTO(
                thesis.Id,
                thesis.Name,
                thesis.Description,
                new TeacherDTO(thesis.Teacher.Id, thesis.Teacher.Name, thesis.Teacher.Email)
            );

            _context.Applies.Add(entity);

            await _context.SaveChangesAsync();

            return (Response.Success, new ApplyDTO(entity.Status, StudentDTO, ThesisDTO));
    }
    public Response Accept(int ThesisID){
        throw new NotImplementedException();
    }

    public void RemoveAllPendings(){
        throw new NotImplementedException();
    }

    public Response RemoveRequest(int ThesisID){
        throw new NotImplementedException();
    }
}