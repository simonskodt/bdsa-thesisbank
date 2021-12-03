namespace Entities.Tests;

public class TeacherRepositoryTest : IDisposable
{
    private readonly ThesisBankContext _context;
    private readonly StudentRepository? _repo_Student;
    private readonly ThesisRepository? _repo_Thesis;
    private readonly TeacherRepository? _repo_Teacher;

    private readonly ApplyRepository? _repo_Apply;


    public TeacherRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();


        Teacher Thore = new Teacher("Thore");
        Thore.Email = "Thore@itu.dk";
        Thore.Id = 1;

        Teacher Rasmus = new Teacher("Rasmus");
        Rasmus.Email = "Rasmus@itu.dk";
        Rasmus.Id = 2;

        context.Teachers.Add(Thore);
        context.Teachers.Add(Rasmus);

        Student Alyson = new Student("Alyson");
        Alyson.Email = "Alyson@mail.dk";
        Alyson.Id = 1;

        Student Victor = new Student("Victor");
        Victor.Email = "Victor@mail.dk";
        Victor.Id = 2;

        context.Students.Add(Alyson);
        context.Students.Add(Victor);

        Apply applies1 = new Apply { Id = 4, Status = Status.Pending, ThesisID = 1, StudentID = 1 };
        Apply applies2 = new Apply { Id = 5, Status = Status.Pending, ThesisID = 2, StudentID = 1 };
        Apply applies3 = new Apply { Id = 3, Status = Status.Pending, ThesisID = 2, StudentID = 2 };
        Apply applies4 = new Apply { Id = 6, Status = Status.Accepted, ThesisID = 5, StudentID = 1 };
        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);
        context.Applies.Add(applies4);


        Thesis WildAlgorithms = new Thesis("WildAlgorithms", Thore);
        WildAlgorithms.Id = 1;
        WildAlgorithms.Description = "This is a Thesis about a very interesting topic";

        Thesis GraphAlgorithms = new Thesis("GraphAlgorithms", Thore);
        WildAlgorithms.Id = 2;
        WildAlgorithms.Description = "This is a Thesis about a very interesting algorithm";

        Thesis Linq = new Thesis("Linq", Rasmus);
        WildAlgorithms.Id = 3;
        WildAlgorithms.Description = "This is a Thesis about a very interesting linq";

        Thesis Migration = new Thesis("Migration", Rasmus);
        WildAlgorithms.Id = 4;
        WildAlgorithms.Description = "This is a Thesis about a very interesting Migration";

        Thesis CSharp = new Thesis("CSharp", Thore);
        WildAlgorithms.Id = 5;
        WildAlgorithms.Description = "This is a Thesis about a very interesting programming language";

        context.Theses.Add(WildAlgorithms);
        context.Theses.Add(GraphAlgorithms);
        context.Theses.Add(Linq);
        context.Theses.Add(Migration);
        context.Theses.Add(CSharp);

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

        var expectedTeacherDTO = new TeacherDTO(1, "Thore", "Thore@itu.dk");

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
    public async Task Reject_GivenStudentID1AndThesisID1_ChangesStatusFromPendingToDenied()
    {
        var ApplyEntry = await _repo_Apply.ReadApplied(1, 1);
        var testStatus = ApplyEntry.Item2.Status;

        var ApplyEntryUpdate = await _repo_Teacher.Reject(1, 1);
        var UpdateStatus = ApplyEntryUpdate.Item2.Status;

        Assert.Equal(Status.Pending, testStatus);
        Assert.Equal(Status.Denied, UpdateStatus);
    }

    [Fact]
    public async Task ReadStudentApplication_GivenTeacher2_ReturnEmptyList()
    {
        var actual = await _repo_Teacher.ReadPendingStudentApplication(2);

        Assert.Empty(actual);
    }

    [Fact]
    public async Task ReadPendingStudentApplication_GivenTeahcerID1_ReturnListOfPendingApplyDTOs(){

        //Arrange
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Student.ReadStudent(1);
        var student_2 = await _repo_Student.ReadStudent(2);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);


        var readList = await _repo_Teacher.ReadPendingStudentApplication(1);

        var DTO_1 = await _repo_Apply.ReadApplied(student.Item2.Id, thesis1.Item2.Id);
        var DTO_2 = await _repo_Apply.ReadApplied(student.Item2.Id, thesis2.Item2.Id);
        var DTO_3 = await _repo_Apply.ReadApplied(student_2.Item2.Id, thesis2.Item2.Id);


        //Assert
        Assert.Collection(readList,
        thesis => Assert.Equal(DTO_1.Item2, thesis),
        thesis => Assert.Equal(DTO_3.Item2, thesis),
        thesis => Assert.Equal(DTO_2.Item2, thesis));

    }

    public void Dispose()
    {
        _context.Dispose();
    }
}