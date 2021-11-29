namespace Entities.Tests;

public class TeacherRepositoryTest : IDisposable
{
    private readonly ThesisBankContext _context;
    private readonly StudentRepository? _repo_Student;
    private readonly ThesisRepository? _repo_Thesis;
    private readonly TeacherRepository? _repo_Teacher;

    private readonly ApplyRepository? _repo_Apply;


    public TeacherRepositoryTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher Thore = new Teacher {Id = 1, Name = "Thore", Email = "Thore@itu.dk"};
        Teacher Rasmus = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};
        context.Teachers.Add(Thore);
        context.Teachers.Add(Rasmus);

        Student Alyson = new Student{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        Student Victor = new Student{Id = 2, Name = "Victor", Email ="Victor@mail.dk"};
        context.Students.Add(Alyson);
        context.Students.Add(Victor);

        Apply applies1 = new Apply{Id =1, Status = Status.Pending, ThesisID = 1, StudentID = 1}; 
        Apply applies2 = new Apply{Id =2, Status = Status.Pending, ThesisID = 2, StudentID = 1}; 
        context.Applies.Add(applies1);
        context.Applies.Add(applies2);

        Apply applies3 = new Apply{Id =3, Status = Status.Pending, ThesisID = 2, StudentID = 2}; 
        context.Applies.Add(applies3);

        context.Theses.Add(new Thesis { Id = 1, Name = "WildAlgorithms", Description ="This is a Thesis about a very interesting topic", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, Name = "GraphAlgorithms", Description ="This is a Thesis about a very interesting algorithm", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 3, Name = "Linq", Description ="This is a Thesis about a very interesting linq", Teacher = Rasmus });
        context.Theses.Add(new Thesis { Id = 4, Name = "Migration",Description ="This is a Thesis about a very interesting Migration", Teacher = Rasmus });


        context.SaveChangesAsync();

        _context = context;
        _repo_Student = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
        _repo_Teacher = new TeacherRepository(_context);
        _repo_Apply = new ApplyRepository(_context);


    }

    /*

    TESTS FOR METHODE ReadTeacher

    */

    [Fact]
    public async Task ReadTeacher_GivenTeacher1_ReturnResonseSuccessAndTeacher1DTO(){
        
        var actual = await _repo_Teacher.ReadTeacher(1);

        var expectedTeacherDTO = new TeacherDTO(1, "Thore", "Thore@itu.dk");

        Assert.Equal((Response.Success, expectedTeacherDTO), actual);
        
    }

    [Fact]
    public async Task ReadTeacher_GivenTeacher3_ReturnResonseNotFoundAndNull(){
        
        var actual = await _repo_Teacher.ReadTeacher(3);

        Assert.Equal((Response.NotFound, null), actual);
        
    }


    [Fact]
    public async Task Accept(){

        //var listofApllication = await _repo_Teacher.ReadStudentApplication(1);
        //længden af den gemmest

        //var AcceptedResponse = await _repo_Teacher.Accept(1,1);

        //Assert.Equal(Response.Success, AcceptedResponse);

        // længden der var gemt skal nu være en mindre.

        var ApplyEntryTest = await _repo_Apply.ReadApplied(1, 1);
        var testStatus = ApplyEntryTest.Item2.Status;

        var ApplyEntryUpdate = await _repo_Teacher.Accept(1,1);
        var UpdateStatus = ApplyEntryUpdate.Item2.Status;

        Assert.Equal(Status.Pending, testStatus);
        Assert.Equal(Status.Accepted, UpdateStatus);
       
    }
    
    [Fact]
    public async Task Reject(){

        var ApplyEntryTest = await _repo_Apply.ReadApplied(1, 1);
        var testStatus = ApplyEntryTest.Item2.Status;

        var ApplyEntryUpdate = await _repo_Teacher.Reject(1,1);
        var UpdateStatus = ApplyEntryUpdate.Item2.Status;

        Assert.Equal(Status.Pending, testStatus);
        Assert.Equal(Status.Denied, UpdateStatus);

    }

    /*

    TESTS FOR METHODE ReadStudentApplication

    */
    [Fact]
    public async Task ReadStudentApplication_GivenTeacher1_ReturnListWith3Applications(){
        var student1 = (await _repo_Student.ReadStudent(1)).Item2; 
        var student2 = (await _repo_Student.ReadStudent(2)).Item2; 
        var thesis1 = (await _repo_Thesis.ReadThesis(1)).Item2; 
        var thesis2 = (await _repo_Thesis.ReadThesis(2)).Item2; 

        var expectedApplyDTO1 = new ApplyDTO(Status.Pending, student1, thesis1);
        var expectedApplyDTO2 = new ApplyDTO(Status.Pending, student1, thesis2);
        var expectedApplyDTO3 = new ApplyDTO(Status.Pending, student2, thesis2);

        var actual = await _repo_Teacher.ReadStudentApplication(1);

        Assert.Collection(actual,
            apply => Assert.Equal(expectedApplyDTO1, apply),
            apply => Assert.Equal(expectedApplyDTO2, apply),
            apply => Assert.Equal(expectedApplyDTO3, apply)
        );

    }

    [Fact]
    public async Task ReadStudentApplication_GivenTeacher2_ReturnEmptyList(){
        
        var actual = await _repo_Teacher.ReadStudentApplication(2);

        Assert.Empty(actual);

    }

    
    public void Dispose(){

        _context.Dispose();

    }
}