namespace Entities.Tests;

public class StudentRepositoryTest : IDisposable
{
    private readonly ThesisBankContext? _context;
    private readonly StudentRepository? _repo_Stud;
    private readonly ThesisRepository? _repo_Thesis;


    public StudentRepositoryTest(){

        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();

        Teacher Thore = new Teacher { Id = 1, Name = "Thore", Email = "Thore@itu.dk"};
        context.Teachers.Add(Thore);
        Student Victor = new Student{Id = 1, Name = "Victor", Email = "Vibr@itu.dk"};
        Student Alyson = new Student{Id = 2, Name = "Alyson", Email = "Alyson@itu.dk"};
        context.Students.Add(Victor);
        context.Theses.Add(new Thesis { Id = 1, Name = "WildAlgorithms", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, Name = "GraphAlgorithms", Teacher = Thore });

        context.SaveChangesAsync();


        _context = context;
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }


    [Fact]
    public async Task ReadStudent_GivenStudent1_ReturnResonseSuccessAndStudent1DTO(){
        
        var actual = await _repo_Stud.ReadStudent(1);

        var expectedStudentDTO = new StudentDTO{Id = 1, Name = "Victor", Email = "Vibr@itu.dk"};

        Assert.Equal((Response.Success, expectedStudentDTO), actual);
    }

    [Fact]
    public async Task ReadStudent_GivenStudent3_ReturnResonseNotFoundAndNull(){
        
        var actual = await _repo_Stud.ReadStudent(3);

        Assert.Equal((Response.NotFound, null), actual);
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis1_ReturnResonseSuccessAndAppliedDTO(){
        
        var student = await _repo_Stud.ReadStudent(1);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(1, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
       // Vi burde måde også tjekke om den nu er inde i Applies table???
       // Assert.Equal(_repo_Thesis.ReadPendingThesis(1))
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent3AndThesis1_ReturnResonseNotFoundAndNull(){

        var readApplied = await _repo_Stud.ApplyForThesis(3, 1);

        Assert.Equal((Response.Success, null), readApplied);
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis3_ReturnResonseNotFoundAndNull(){

        var readApplied = await _repo_Stud.ApplyForThesis(1, 3);

        Assert.Equal((Response.Success, null), readApplied);
    }


    [Fact]
    public async Task Accept_GivenSudent1AndThesis1_ReturnResonseSuccessAndThesis1DTO(){

        var student = await _repo_Stud.ReadStudent(1);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedAccepted = new ApplyDTO(Status.Accepted, student.Item2, thesis.Item2);

        var readAccepted = await _repo_Stud.Accept(1,1);

        Assert.Equal((Response.Success, expectedAccepted), readAccepted);

    }

    [Fact]
    public async Task RemoveAllPendings_GivenStudent1_ReturnsDeleted(){
        
        var readResponse = await _repo_Stud.RemoveAllPendings(1);

        Assert.Equal(Response.Deleted, readResponse);
        Assert.Empty((await _repo_Thesis.ReadPendingThesis(1)));
        
    }

    [Fact]
    public async Task RemoveRequest_GivenThesis1_ReturnResponseSuccessAndThesisDTOWithThesis1(){

        var expectedRemovedThesisDTO =  (await _repo_Thesis.ReadThesis(1)).Item2;

        var readRemoved = await _repo_Stud.RemoveRequest(1,1);

        Assert.Equal((Response.Deleted, expectedRemovedThesisDTO), readRemoved);

    }

    public void Dispose(){

        _context.Dispose();

    }
}