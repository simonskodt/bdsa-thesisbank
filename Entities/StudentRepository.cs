namespace Entities;
public class StudentRepository : IStudentRepository
{
    ThesisBankContext _context;
    ThesisRepository _repo_Thesis;

    public StudentRepository(ThesisBankContext context){
        _context = context;
        _repo_Thesis = new ThesisRepository(_context);
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
            
            var student = await _context.Students
                            .Where(s => s.Id == studentID)
                            .FirstOrDefaultAsync();

            var thesis = await _context.Theses
                            .Where(t => t.Id == ThesisID)
                            .FirstOrDefaultAsync();


            if(student == null || thesis == null){
                return (Response.NotFound, null);
            }               

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

    public async Task<(Response, ApplyDTO)> Accept(int ThesisID, int StudentID){
        
        var apllies = await _context.Applies
                        .Where(a => a.StudentID == StudentID)
                        .Where(a => a.ThesisID == StudentID)
                        .Where(a => a.Status == Status.Accepted)
                        .FirstOrDefaultAsync();

        if(apllies == null){
            return (Response.NotFound, null);
        }
        apllies.Status = Status.Archived;

        await _context.SaveChangesAsync();
        var resp_stud = await ReadStudent(StudentID);
        var resp_thes = await _repo_Thesis.ReadThesis(ThesisID);
        var apply_dto = new ApplyDTO(Status.Archived, resp_stud.Item2, resp_thes.Item2);

        return (Response.Updated, apply_dto);
    }

    public async Task<Response> RemoveRequest(int ThesisID, int StudentID){

        var pending = await _context.Applies
                        .Where(p => p.StudentID == StudentID)
                        .Where(p => p.ThesisID == ThesisID)
                        .Where(p => p.Status == Status.Pending)
                        .FirstOrDefaultAsync();
                        

        _context.Applies.Remove(pending);

        await _context.SaveChangesAsync(); 
        return Response.Deleted;
    }

    public async Task<Response> RemoveAllPendings(int StudentID){
        var allPending = await _context.Applies
                        .Where(p => p.StudentID == StudentID)
                        .Where(p => p.Status == Status.Pending)
                        .ToListAsync();

        _context.Applies.RemoveRange(allPending);

        await _context.SaveChangesAsync(); 
        return Response.Deleted;    }

}