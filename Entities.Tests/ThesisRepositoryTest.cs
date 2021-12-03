
namespace Entities.Tests;

public class ThesisRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    private readonly StudentRepository? _repo_Stud;
    private readonly ThesisRepository? _repo_Thesis;

    public ThesisRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher Thore = new Teacher("Thore","Thore@itu.dk"){Id = 1};
        Teacher Rasmus = new Teacher("Rasmus", "Rasmus@itu.dk"){Id = 2};
        context.Teachers.Add(Thore);
        context.Teachers.Add(Rasmus);

        Student Alyson = new Student("Alyson", "Alyson@mail.dk"){Id = 1};
        Student Victor = new Student("Victor", "Victor@mail.dk"){Id = 2};
        context.Students.Add(Alyson);
        context.Students.Add(Victor);

        Thesis WildAlgorithms = new Thesis("WildAlgorithms", Thore) {Id = 1, Description = "This is a Thesis about a very interesting topic"};
        Thesis GraphAlgorithms = new Thesis("GraphAlgorithms", Thore){Id = 2, Description = "This is a Thesis about a very interesting algorithm"};
        Thesis Linq = new Thesis("Linq", Rasmus){Id = 3, Description = "This is a Thesis about a very interesting linq"};
        Thesis Migration = new Thesis("Migration", Rasmus){Id = 4, Description = "This is a Thesis about a very interesting Migration"};


        context.Theses.Add(WildAlgorithms);
        context.Theses.Add(GraphAlgorithms);
        context.Theses.Add(Linq);
        context.Theses.Add(Migration);


        Apply applies1 = new Apply(WildAlgorithms, Alyson){Id =1}; 
        Apply applies2 = new Apply(GraphAlgorithms, Alyson){Id =2}; 
        Apply applies3 = new Apply(Linq, Alyson){Id =3}; 
        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);


        context.SaveChangesAsync();

        _context = context;
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }

    /*

    TESTS FOR METHODE ReadThesis

    */

    
    [Fact]
    public async Task ReadThesis_GivenID1_ReturnWildAlgorithmsByThore()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo_Thesis.ReadThesis(1);

        Assert.Equal((Response.Success,new ThesisDTO(1,"WildAlgorithms", "This is a Thesis about a very interesting topic", Thore)), ReadThesisResponse);
    }

    [Fact] 
    public async Task ReadThesis_GivenNonExisitingID_ReturnEmpty()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo_Thesis.ReadThesis(8);

        Assert.Equal((Response.NotFound,null), ReadThesisResponse);


    }

    /*

    TESTS FOR METHODE ReadAllTheses

    */

    [Fact]
    public async Task ReadAllTheses_GivenNoParameter_ReturnAllTheses()
    {
        
        MinimalThesisDTO t1 = new MinimalThesisDTO(1, "WildAlgorithms", "This is a Thesis about a very interesting topic", "Thore");
        MinimalThesisDTO t2 = new MinimalThesisDTO(2, "GraphAlgorithms", "This is a Thesis about a very interesting algorithm", "Thore");
        MinimalThesisDTO t3 = new MinimalThesisDTO(3, "Linq","This is a Thesis about a very interesting linq", "Rasmus");
        MinimalThesisDTO t4 = new MinimalThesisDTO(4, "Migration","This is a Thesis about a very interesting Migration", "Rasmus");
        
        
        IReadOnlyCollection<MinimalThesisDTO> ReadThesisResponse = await _repo_Thesis.ReadAll();

        Assert.Collection(ReadThesisResponse,
            thesis => Assert.Equal(t1, thesis),
            thesis => Assert.Equal(t2, thesis),
            thesis => Assert.Equal(t3, thesis),
            thesis => Assert.Equal(t4, thesis)
        );
    }

    /*

    TESTS FOR METHODE ReadAppliedThesis

    */

    [Fact]
    public async Task ReadAppliedThesis_GivenStudentId1_ReturnAppliedThesis()
    {
        var student1 = (await _repo_Stud.ReadStudent(1)).Item2;
        var thesis1 = (await _repo_Thesis.ReadThesis(1)).Item2;
        var thesis2 = (await _repo_Thesis.ReadThesis(2)).Item2;
        var thesis3 = (await _repo_Thesis.ReadThesis(3)).Item2;

        ApplyDTO a1 = new ApplyDTO(Status.Pending, student1, thesis1);
        ApplyDTO a2 = new ApplyDTO(Status.Pending, student1, thesis2);
        ApplyDTO a3 = new ApplyDTO(Status.Pending, student1, thesis3);

        
        IReadOnlyCollection<ThesisDTO> ReadAppliedThesisResponse = await _repo_Thesis.ReadPendingThesis(1);
        
        Assert.Collection(ReadAppliedThesisResponse,
            thesis => Assert.Equal(thesis1, thesis),
            thesis => Assert.Equal(thesis2, thesis),
            thesis => Assert.Equal(thesis3, thesis)
        );
    } 

    [Fact]
    public async Task ReadAppliedThesis_GivenStudentId2_ReturnEmptyList()
    {
        var student1 = (await _repo_Stud.ReadStudent(2)).Item2;
        
        IReadOnlyCollection<ThesisDTO> ReadAppliedThesisResponse = await _repo_Thesis.ReadPendingThesis(2);
        
        Assert.Empty(ReadAppliedThesisResponse);
    } 

     public void Dispose()
    {
        _context.Dispose();
    }

}

