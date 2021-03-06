namespace Entities.Tests;

public class TeacherRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    readonly StudentRepository _repo_Student;
    readonly ThesisRepository _repo_Thesis;
    readonly TeacherRepository _repo_Teacher;
    readonly ApplyRepository _repo_Apply;

    public TeacherRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();

        Teacher thore = new Teacher("Thore", "thore@itu.dk") { Id = 1 };
        Teacher rasmus = new Teacher("Rasmus", "ramu@itu.dk") { Id = 2 };

        context.Teachers.Add(thore);
        context.Teachers.Add(rasmus);

        Student alyson = new Student("Alyson", "alyson@mail.dk") { Id = 1 };
        Student victor = new Student("Victor", "victor@mail.dk") { Id = 2 };

        context.Students.Add(alyson);
        context.Students.Add(victor);

        Thesis wildAlgorithms = new Thesis("WildAlgorithms", 1) { Id = 1, Description = "This is a Thesis about a very interesting topic" };
        Thesis graphAlgorithms = new Thesis("GraphAlgorithms", 1) { Id = 2, Description = "This is a Thesis about a very interesting algorithm" };
        Thesis linq = new Thesis("Linq", 2) { Id = 3, Description = "This is a Thesis about a very interesting linq" };
        Thesis migration = new Thesis("Migration", 2) { Id = 4, Description = "This is a Thesis about a very interesting migration" };
        Thesis cSharp = new Thesis("CSharp", 1) { Id = 5, Description = "This is a Thesis about a very interesting programming language" };

        context.Theses.Add(wildAlgorithms);
        context.Theses.Add(graphAlgorithms);
        context.Theses.Add(linq);
        context.Theses.Add(migration);
        context.Theses.Add(cSharp);

        Apply applies1 = new Apply(1, 1) { Id = 1 };
        Apply applies2 = new Apply(2, 1) { Id = 2 };
        Apply applies3 = new Apply(2, 2) { Id = 3 };
        Apply applies4 = new Apply(5, 1) { Id = 4, Status = Status.Accepted };

        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);
        context.Applies.Add(applies4);

        context.SaveChangesAsync();

        _context = context;
        _repo_Student = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
        _repo_Teacher = new TeacherRepository(_context);
        _repo_Apply = new ApplyRepository(_context);
    }

    [Fact]
    public async Task ReadTeacher_GivenTeacher1_ReturnsResponseSuccessAndTeacher1DTO()
    {
        var actual = await _repo_Teacher.ReadTeacher(1);

        var expectedTeacherDTO = new TeacherDTO(1, "Thore", "thore@itu.dk");

        Assert.Equal((Response.Success, expectedTeacherDTO), actual);
    }

    [Fact]
    public async Task ReadTeacher_GivenTeacher3_ReturnsRepsonseNotFoundAndNull()
    {
        var actual = await _repo_Teacher.ReadTeacher(3);

        Assert.Equal((Response.NotFound, null), actual);
    }


    [Fact]
    public async Task Accept_GivenStudentID1AndThesisID1_ChangesStatusFromPendingToAccepted()
    {
        var applyEntry = await _repo_Apply.ReadApplied(1, 1);
        var status = applyEntry.Item2.Status;

        var applyEntryUpdate = await _repo_Teacher.Accept(1, 1);
        var updateStatus = applyEntryUpdate.Item2.Status;

        Assert.Equal(Status.Pending, status);
        Assert.Equal(Status.Accepted, updateStatus);
    }

    [Fact]
    public async Task Accept_GivenStudentID1AndThesisID1_ReturnsResponceSucces()
    {
        var appliedChange = await _repo_Teacher.Accept(1, 1);
        var responseNotChanged = appliedChange.Item1;

        Assert.Equal(Response.Success, responseNotChanged);
    }

    [Fact]
    public async Task Accept_GivenNonExistingStudentIDAndNonExistingThesisID_ReturnsResponseNotFound()
    {
        var applied = await _repo_Apply.ReadApplied(10, 10);
        var response = applied.Item1;

        var appliedChange = await _repo_Teacher.Accept(10, 10);
        var responseNotChanged = appliedChange.Item1;

        Assert.Equal(Response.NotFound, response);
        Assert.NotEqual(Response.Success, responseNotChanged);
    }


    [Fact]
    public async Task ReadStudentApplication_GivenTeacher2_ReturnEmptyList()
    {
        var actual = await _repo_Teacher.ReadPendingApplicationsByTeacherID(2);

        Assert.Empty(actual);
    }

    [Fact]
    public async Task ReadPendingStudentApplication_GivenTeahcerID1_ReturnListOfPendingApplyDTOs()
    {
        //Arrange
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Student.ReadStudent(1);
        var student_2 = await _repo_Student.ReadStudent(2);
        var thesis1 = new ThesisDTOMinimal(1, "WildAlgorithms", null, "Thore");
        var thesis2 = new ThesisDTOMinimal(2, "GraphAlgorithms", null, "Thore");


        var readList = await _repo_Teacher.ReadPendingApplicationsByTeacherID(1);

        var expected_DTO_1 = new ApplyDTOWithMinalThesis(1, Status.Pending, student.Item2, thesis1);
        var expected_DTO_2 = new ApplyDTOWithMinalThesis(2, Status.Pending, student.Item2, thesis2);
        var expected_DTO_3 = new ApplyDTOWithMinalThesis(3, Status.Pending, student_2.Item2, thesis2);

        //Assert
        Assert.Collection(readList,
        thesis => Assert.Equal(expected_DTO_1, thesis),
        thesis => Assert.Equal(expected_DTO_2, thesis),
        thesis => Assert.Equal(expected_DTO_3, thesis));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}