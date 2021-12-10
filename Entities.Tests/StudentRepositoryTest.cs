namespace Entities.Tests;

public class StudentRepositoryTest : IDisposable
{
    private readonly ThesisBankContext _context;
    private readonly StudentRepository _repo_Stud;
    private readonly ThesisRepository _repo_Thesis;

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
        context.Applies.Add(new Apply(3, 2) { Id = 3, Status = Status.Denied });
        context.Applies.Add(new Apply(2, 1) { Id = 4, Status = Status.Archived });
        context.Applies.Add(new Apply(1, 3) { Id = 5 });
        context.Applies.Add(new Apply(2, 3) { Id = 6 });
        context.Applies.Add(new Apply(3, 3) { Id = 7 });

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
    public async Task ReadStudentIDByName_GivenAlyson_ReturnsId1(){

        var actual = await _repo_Stud.ReadStudentIDByName("Alyson");

        Assert.Equal((Response.Success, 1), actual);

    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis1_ReturnResonseSuccessAndAppliedDTO()
    {
        var student = await _repo_Stud.ReadStudent(1);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(1, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent9AndThesis1_ReturnResonseNotFoundAndNull()
    {
        var readApplied = await _repo_Stud.ApplyForThesis(9, 1);

        Assert.Equal((Response.NotFound, null), readApplied);
    }


    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis9_ReturnResonseNotFoundAndNull()
    {
        var readApplied = await _repo_Stud.ApplyForThesis(1, 9);

        Assert.Equal((Response.NotFound, null), readApplied);
    }

    // to check that it is posible for two different students to apply for the same thesis
    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent2AndThesis1_ReturnResonseSuccessAndAppliedDTO()
    {
        var student = await _repo_Stud.ReadStudent(2);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(2, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
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
    public async Task RemoveAllPendings_GivenStudent1_ReturnsDeleted()
    {
        var readResponse = await _repo_Stud.RemoveAllPendings(1);

        Assert.Equal(Response.Deleted, readResponse);
    }

    [Fact]
    public async Task RemoveAllPendings_GivenStudentId3_ReturnDeleted()
    {
        var readAllRemoved = await _repo_Stud.RemoveAllPendings(3);

        Assert.Equal(Response.Deleted, readAllRemoved);
        Assert.Empty((await _repo_Thesis.ReadPendingThesis(3)));
    }

    [Fact]
    public async Task RemoveRequest_GivenThesis1_ReturnResponseSuccessAndThesisDTOWithThesis1()
    {
        var readRemoved = await _repo_Stud.RemoveRequest(1, 1);

        Assert.Equal(Response.Deleted, readRemoved);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}