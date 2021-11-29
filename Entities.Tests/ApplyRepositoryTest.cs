namespace Entities.Tests;
public class ApplyRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    private readonly ApplyRepository? _repo_apply;

    private readonly StudentRepository? _repo_Stud;
    private readonly ThesisRepository? _repo_Thesis;

    public ApplyRepositoryTest()
    {
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

        Apply applies1 = new Apply{Id =1, ThesisID = 1, StudentID = 1}; 
        Apply applies2 = new Apply{Id =2, ThesisID = 2, StudentID = 1}; 
        Apply applies3 = new Apply{Id =3, ThesisID = 3, StudentID = 1}; 
        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);

        context.Theses.Add(new Thesis { Id = 1, Name = "WildAlgorithms", Description ="This is a Thesis about a very interesting topic", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, Name = "GraphAlgorithms", Description ="This is a Thesis about a very interesting algorithm", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 3, Name = "Linq", Description ="This is a Thesis about a very interesting linq", Teacher = Rasmus });
        context.Theses.Add(new Thesis { Id = 4, Name = "Migration",Description ="This is a Thesis about a very interesting Migration", Teacher = Rasmus });

        context.SaveChangesAsync();

        _context = context;
        _repo_apply = new ApplyRepository(_context);
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }
    [Fact]
    public async Task Given_Student_ID_1_and_Thesis_Id_1_Returns_Applied_id_1(){
        
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        StudentDTO Alyson = new StudentDTO{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        ThesisDTO Thesis1 = new ThesisDTO(1, "WildAlgorithms", "This is a Thesis about a very interesting topic", Thore);
        
        var ExpectedDTO = new ApplyWithIDDTO(1, Status.Pending, Alyson, Thesis1);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 1);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }
    
    public void Dispose(){

        _context.Dispose();

    }


}