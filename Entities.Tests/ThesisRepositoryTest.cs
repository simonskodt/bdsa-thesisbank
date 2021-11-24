namespace Entities.Tests;

public class ThesisRepositoryTest : IDisposable
{
    private readonly ThesisBankContext _context;
    private readonly ThesisRepository _repo;

    public ThesisRepositoryTest()
    {

        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher Thore = new Teacher { Id = 1, name = "Thore", email = "Thore@itu.dk"};
        context.Teachers.Add(Thore);
        Teacher Rasmus = new Teacher{Id = 2, name = "Rasmus", email = "Rasmus@itu.dk"};
        context.Teachers.Add(Rasmus);
        context.Theses.Add(new Thesis { Id = 1, name = "WildAlgorithms", teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, name = "GraphAlgorithms", teacher = Thore });
        context.Theses.Add(new Thesis { Id = 3, name = "Linq", teacher = Rasmus });
        context.Theses.Add(new Thesis { Id = 4, name = "Migration", teacher = Rasmus });


        context.SaveChanges();

        _context = context;
        _repo = new ThesisRepository(_context);
    }

    
    [Fact]
    public void ReadThesis_GivenID1_ReturnWildAlgorithmsByThore()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = _repo.ReadThesis(1);

        Assert.Equal((Response.Success,new ThesisDTO(1, "WildAlgorithms", Thore)), ReadThesisResponse);


    }

    [Fact] 
    public void ReadThesis_GivenNonExisitingID_ReturnEmpty()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = _repo.ReadThesis(8);

        Assert.Equal((Response.NotFound,null), ReadThesisResponse);

    }

    [Fact]
    public void ReadAllTheses_GivenNoParameter_ReturnAllTheses()
    {
        
        MinimalThesisDTO t1 = new MinimalThesisDTO(1, "WildAlgorithms", "Thore");
        MinimalThesisDTO t2 = new MinimalThesisDTO(2, "GraphAlgorithms", "Thore");
        MinimalThesisDTO t3 = new MinimalThesisDTO(3, "Linq", "Rasmus");
        MinimalThesisDTO t4 = new MinimalThesisDTO(4, "Migration", "Rasmus");
        var ExpectedList = new List<MinimalThesisDTO>(){t1,t2,t3,t4}.AsReadOnly();
        
        
        (Response, IReadOnlyCollection<MinimalThesisDTO>) ReadThesisResponse = _repo.ReadAll();

        Assert.Collection(ReadThesisResponse,
            thesis => Assert.Equal((Response.Success, t1), thesis),
            thesis => Assert.Equal((Response.Success, t2), thesis),
            thesis => Assert.Equal((Response.Success, t3), thesis),
            thesis => Assert.Equal((Response.Success, t4), thesis)
        );
    }




    public void Dispose()
    {

        _context.Dispose();

    }
}
