namespace Entities;
public class ApplyRepository : IApplyRepository
{
    ThesisBankContext _context;

    public ApplyRepository(ThesisBankContext context)
    {
        _context = context;
    }

    public async Task<(Response, ApplyDTO?)> ReadApplied(int studentID, int thesisID)
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

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadApplicationsByTeacherID(int teacherID)
    {
        var stud_repo = new StudentRepository(_context);
        var thesis_repo = new ThesisRepository(_context);

        var thesesWithCurrentTeacherID = await _context.Theses
                                                .Where(t => t.Teacher.Id == teacherID)
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

    public async Task<IReadOnlyCollection<ApplyDTO>> ReadAppliedByStudentAndStatus(int StudentID, Status status)
    {
        var stud_repo = new StudentRepository(_context);

        var studentDTO = await stud_repo.ReadStudent(StudentID);
                            
        var Applications = await _context.Applies
                        .Where(a => a.Status == status)
                        .Where(a => a.StudentID == studentDTO.Item2.Id)
                        .Select(a => new ThesisDTO(a.ThesisID, a.Thesis.Name, a.Thesis.Description, new TeacherDTO(a.Thesis.Teacher.Id, a.Thesis.Teacher.Name, a.Thesis.Teacher.Email)))
                        .ToListAsync();

        var ApplyDTOs = new List<ApplyDTO>();


        foreach (var thesis in Applications)
        {
            var DTO = new ApplyDTO(Status.Pending, studentDTO.Item2, thesis);
            ApplyDTOs.Add(DTO);
        }

        return ApplyDTOs.AsReadOnly();

        // var student = await _context.Students
        //                    .Where(s => s.Id == studentID)
        //                    .FirstOrDefaultAsync();

        // var thesis = await _context.Theses
        //                 .Where(t => t.Id == ThesisID)
        //                 .FirstOrDefaultAsync();


        // if (student == null || thesis == null)
        // {
        //     return (Response.NotFound, null);
        // }

        // var entity = new Apply(thesis, student);

        // var StudentDTO = new StudentDTO(student.Id, student.Name, student.Email);

        // var ThesisDTO = new ThesisDTO(thesis.Id, thesis.Name, thesis.Description,
        //                             new TeacherDTO(thesis.Teacher.Id, thesis.Teacher.Name, thesis.Teacher.Email)
        // );

        // _context.Applies.Add(entity);

        // await _context.SaveChangesAsync();

        // return (Response.Success, new ApplyDTO(entity.Status, StudentDTO, ThesisDTO));

    }

    public async Task<(Response, ApplyDTOid?)> ApplyForThesis(int studentID, int ThesisID)
    {
        Console.WriteLine("Student: " + studentID);
        Console.WriteLine("Thesis: " + ThesisID);

        var student = await _context.Students
                           .Where(s => s.Id == studentID)
                           .FirstOrDefaultAsync();


        var thesis = await _context.Theses
                        .Where(t => t.Id == ThesisID)
                        .FirstOrDefaultAsync();

        Console.WriteLine("StudentID: " + student.Id);
        Console.WriteLine("ThesisID: " + thesis.Id);

        if (student == null || thesis == null)
        {
            return (Response.NotFound, null);
        }

        Console.WriteLine("Student ID" + student.Id);

        var entity = new Apply(thesis, student);

        Console.WriteLine(entity.Id);
        Console.WriteLine(entity.Status);
        Console.WriteLine(entity.ThesisID);
        Console.WriteLine(entity.StudentID);
        

        var StudentDTO = new StudentDTO(student.Id, student.Name, student.Email);

        var ThesisDTO = new ThesisDTO(thesis.Id, thesis.Name, thesis.Description,
                                    new TeacherDTO(thesis.Teacher.Id, thesis.Teacher.Name, thesis.Teacher.Email)
        );

        _context.Applies.Add(entity);

        await _context.SaveChangesAsync();

        // return (Response.Success, new ApplyWithIDDTO(entity.Id, entity.Status, StudentDTO, ThesisDTO));
        return (Response.Success, new ApplyDTOid(entity.Status, studentID, ThesisID));
    }

}

