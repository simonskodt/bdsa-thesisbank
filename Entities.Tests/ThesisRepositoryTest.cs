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

        Teacher thore = new Teacher { Id = 1, Name = "Thore", Email = "thore@itu.dk"};
        Teacher niels = new Teacher { Id = 2, Name = "Niels", Email = "nija@itu.dk"};
        
        foreach (var teacher in context.Teachers)
            context.Teachers.Add(teacher);

        foreach (var thesis in context.Theses)
        {
            //context.Thesis.A
        }
        //context.Theses.Add(new Thesis { Id = 1, name = "WildAlgorithms", teacher = Thore });


        context.SaveChanges();

        _context = context;
        _repo = new ThesisRepository(_context);
    }

    
    [Fact]
    public void Read_Thesis_given_ThesisID()
    {
        TeacherDTO teacher = new TeacherDTO(1, "Thore", "thore@itu.dk");
        var thesis = _repo.ReadThesis(1);

        Assert.Equal(new ThesisDTO(1, "WildAlgorithms", teacher), thesis);
    }

    [Fact]
    public void Read_Thesis_given_incorrect_ThesisID_Returns_false()
    {
        TeacherDTO teacher = new TeacherDTO(2, "Niels", "nija@itu.dk");
        var thesis = _repo.ReadThesis(3);

        //Assert.NotEqual(new ThesisDTO)
    }

    public void Dispose()
    {

        _context.Dispose();

    }
}
