namespace Entities.Tests;

public class ThesisRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    private readonly StudentRepository _repo_Stud;
    private readonly ThesisRepository _repo_Thesis;

    public ThesisRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher thore = new Teacher("Thore", "Thore@itu.dk") { Id = 1 };
        Teacher rasmus = new Teacher("Rasmus", "Rasmus@itu.dk") { Id = 2 };

        context.Teachers.Add(thore);
        context.Teachers.Add(rasmus);

        Student alyson = new Student("Alyson", "Alyson@mail.dk") { Id = 1 };
        Student victor = new Student("Victor", "Victor@mail.dk") { Id = 2 };

        context.Students.Add(alyson);
        context.Students.Add(victor);

        Thesis wildAlgorithms = new Thesis("WildAlgorithms", 1) { Id = 1, Description = "This is a Thesis about a very interesting topic" };
        Thesis graphAlgorithms = new Thesis("GraphAlgorithms", 1) { Id = 2, Description = "This is a Thesis about a very interesting algorithm" };
        Thesis linq = new Thesis("Linq", 2) { Id = 3, Description = "This is a Thesis about a very interesting linq" };
        Thesis migration = new Thesis("Migration", 2) { Id = 4, Description = "This is a Thesis about a very interesting Migration" };

        context.Theses.Add(wildAlgorithms);
        context.Theses.Add(graphAlgorithms);
        context.Theses.Add(linq);
        context.Theses.Add(migration);

        Apply applies1 = new Apply(1, 1) { Id = 1 };
        Apply applies2 = new Apply(2, 1) { Id = 2 };
        Apply applies3 = new Apply(3, 1) { Id = 3 };

        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);

        context.SaveChangesAsync();

        _context = context;
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }

    [Fact]
    public async Task ReadThesis_GivenID1_ReturnWildAlgorithmsByThore()
    {
        var Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo_Thesis.ReadThesis(1);

        Assert.Equal((Response.Success, new ThesisDTO(1, "WildAlgorithms", "This is a Thesis about a very interesting topic", Thore)), ReadThesisResponse);
    }

    [Fact]
    public async Task ReadThesis_GivenNonExisitingID_ReturnEmpty()
    {
        var Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        var ReadThesisResponse = await _repo_Thesis.ReadThesis(8);

        Assert.Equal((Response.NotFound, null), ReadThesisResponse);
    }


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

