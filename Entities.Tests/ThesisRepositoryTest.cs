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
        context.Theses.Add(new Thesis { Id = 1, name = "WildAlgorithms", teacher = Thore });


        context.SaveChanges();

        _context = context;
        _repo = new ThesisRepository(_context);
    }

    
    [Fact]
    public void Read_Thesis_given_ThesisID()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var Thesis = _repo.ReadThesis(1);

        Assert.Equal(new ThesisDTO(1, "WildAlgorithms", Thore), Thesis);


    }

    public void Dispose()
    {

        _context.Dispose();

    }
}
