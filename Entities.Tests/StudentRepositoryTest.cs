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
    public async Task ApplyForThesis_GivenAppliedStudent1_ReturnResonseSuccessAndAppliedDTO(){
        
        var student = await _repo_Stud.ReadStudent(1);

        var thesis = await _repo_Thesis.ReadThesis(1);

        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(1, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
    }

    public void Dispose(){

        _context.Dispose();

    }
}