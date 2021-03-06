namespace Entities.Tests;

public class StudentRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    readonly StudentRepository _repo_Stud;
    readonly ThesisRepository _repo_Thesis;

    public StudentRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();

        Teacher thore = new Teacher("Thore", "thore@itu.dk") { Id = 1 };
        context.Teachers.Add(thore);

        Student alyson = new Student("Alyson", "alyson@mail.dk") { Id = 1 };
        Student victor = new Student("Victor", "victor@mail.dk") { Id = 2 };
        Student leonora = new Student("Leonora", "leonora@itu.dk") { Id = 3 };
        context.Students.Add(alyson);
        context.Students.Add(victor);
        context.Students.Add(leonora);

        Thesis wildAlgorithms = new Thesis("WildAlgorithms", 1) { Id = 1, Description = "This is a Thesis about a very interesting topic" };
        Thesis graphAlgorithms = new Thesis("GraphAlgorithms", 1) { Id = 2, Description = "This is a Thesis about a very interesting algorithm" };
        Thesis designingUI = new Thesis("DesigningUI", 1) { Id = 3 };

        context.Theses.Add(wildAlgorithms);
        context.Theses.Add(graphAlgorithms);
        context.Theses.Add(designingUI);

        context.Applies.Add(new Apply(1, 1) { Id = 1 });
        context.Applies.Add(new Apply(2, 2) { Id = 2, Status = Status.Accepted });
        context.Applies.Add(new Apply(3, 2) { Id = 3, Status = Status.Archived });
        context.Applies.Add(new Apply(2, 1) { Id = 4, Status = Status.Archived });
        context.Applies.Add(new Apply(1, 3) { Id = 5 });
        context.Applies.Add(new Apply(2, 3) { Id = 6 });
        context.Applies.Add(new Apply(3, 3) { Id = 7 });
        context.Applies.Add(new Apply(3, 1) { Id = 8, Status = Status.Accepted });

        context.SaveChangesAsync();

        _context = context;
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }

    [Fact]
    public async Task ReadStudent_GivenStudent1_ReturnResonseSuccessAndStudent1DTO()
    {
        var actual = await _repo_Stud.ReadStudent(1);

        var expectedStudentDTO = new StudentDTO(1, "Alyson", "alyson@mail.dk");

        Assert.Equal((Response.Success, expectedStudentDTO), actual);
    }

    [Fact]
    public async Task ReadStudent_GivenStudent9_ReturnResonseNotFoundAndNull()
    {
        var actual = await _repo_Stud.ReadStudent(9);

        Assert.Equal((Response.NotFound, null), actual);
    }

    [Fact]
    public async Task ReadStudentIDByName_GivenAlyson_ReturnsId1()
    {

        var actual = await _repo_Stud.ReadStudentIDByName("Alyson");

        Assert.Equal((Response.Success, 1), actual);

    }

    [Theory]
    [InlineData(false, Response.Updated, Status.Archived, 2, 2)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    public async Task Accept_GivenStudentThesis_ReturnStatus(Boolean isNullDTO, Response expectedResponse, Status expectedStatus, int expectedStudentID, int expectedThesisID)
    {
        var student = await _repo_Stud.ReadStudent(expectedStudentID);
        var thesis = await _repo_Thesis.ReadThesis(expectedThesisID);

        ApplyDTO? expectedApplied;

        if (isNullDTO)
        {
            expectedApplied = null;
        }
        else
        {
            expectedApplied = new ApplyDTO(expectedStatus, student.Item2, thesis.Item2);
        }

        var readApplied = await _repo_Stud.Accept(expectedStudentID, expectedThesisID);

        Assert.Equal((expectedResponse, expectedApplied), readApplied);
    }

    [Fact]
    public async Task Accept_Test()
    {
        var actual = await _repo_Stud.Accept(1, 3);
        Assert.Equal(Status.Archived, actual.Item2.Status);
    }


    [Fact]
    public async Task RemoveAllPendings_GivenStudent1_ReturnsDeleted()
    {
        var readResponse = await _repo_Stud.RemoveAllApplications(1);

        Assert.Equal(Response.Deleted, readResponse);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}