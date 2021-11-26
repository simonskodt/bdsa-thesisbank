namespace Entities.Tests;
public class ThesisRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    readonly ThesisRepository _repo;

    public ThesisRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher Thore = new Teacher { Id = 1, Name = "Thore", Email = "Thore@itu.dk"};
        context.Teachers.Add(Thore);
        Teacher Rasmus = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};
        context.Teachers.Add(Rasmus);
        context.Theses.Add(new Thesis { Id = 1, Name = "WildAlgorithms", Description ="This is a Thesis about a very interesting topic", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, Name = "GraphAlgorithms", Description ="This is a Thesis about a very interesting algorithm", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 3, Name = "Linq", Description ="This is a Thesis about a very interesting linq", Teacher = Rasmus });
        context.Theses.Add(new Thesis { Id = 4, Name = "Migration",Description ="This is a Thesis about a very interesting Migration", Teacher = Rasmus });


        context.SaveChangesAsync();

        _context = context;
        _repo = new ThesisRepository(_context);
    }

    
    [Fact]
    public async Task ReadThesis_GivenID1_ReturnWildAlgorithmsByThore()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo.ReadThesis(1);

        Assert.Equal((Response.Success,new ThesisDTO(1,"WildAlgorithms", "This is a Thesis about a very interesting topic", Thore)), ReadThesisResponse);
    }

    [Fact] 
    public async Task ReadThesis_GivenNonExisitingID_ReturnEmpty()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo.ReadThesis(8);

        Assert.Equal((Response.NotFound,null), ReadThesisResponse);

    }

    [Fact]
    public async Task ReadAllTheses_GivenNoParameter_ReturnAllTheses()
    {
        
        MinimalThesisDTO t1 = new MinimalThesisDTO(1, "WildAlgorithms", "This is a Thesis about a very interesting topic", "Thore");
        MinimalThesisDTO t2 = new MinimalThesisDTO(2, "GraphAlgorithms", "This is a Thesis about a very interesting algorithm", "Thore");
        MinimalThesisDTO t3 = new MinimalThesisDTO(3, "Linq","This is a Thesis about a very interesting linq", "Rasmus");
        MinimalThesisDTO t4 = new MinimalThesisDTO(4, "Migration","This is a Thesis about a very interesting Migration", "Rasmus");

        var ExpectedList = new List<MinimalThesisDTO>(){t1,t2,t3,t4}.AsReadOnly();
        
        
        IReadOnlyCollection<MinimalThesisDTO> ReadThesisResponse = await _repo.ReadAll();

        Assert.Collection(ReadThesisResponse,
            thesis => Assert.Equal(t1, thesis),
            thesis => Assert.Equal(t2, thesis),
            thesis => Assert.Equal(t3, thesis),
            thesis => Assert.Equal(t4, thesis)
        );
    }



    //[Fact]
/*     public async Task ReadAppliedThesis_GivenStudentId1_ReturnAppliedThesis()
    {
        //AppliedDTO 
        ApplyDTO a1 = new ApplyDTO(ID, Status, ThesisID, Thesis);
        ApplyDTO a1 = new ApplyDTO(1, "applied", 4);
        IReadOnlyCollection<ThesisDTO> ReadAppliedThesisResponse = await _repo.ReadAppliedThesis(1);
        Assert.Collection(ReadAppliedThesisResponse,
            thesis => Assert.Equal(a1, thesis),
            thesis => Assert.Equal(a2, thesis)
        );
    } */

    public void Dispose()
    {
        _context.Dispose();
    }
}