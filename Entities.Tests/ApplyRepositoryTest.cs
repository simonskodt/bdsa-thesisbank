namespace Entities.Tests;
public class ApplyRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    private readonly ApplyRepository _repo_apply;
    private readonly StudentRepository _repo_Stud;
    private readonly ThesisRepository _repo_Thesis;
    private readonly TeacherRepository _repo_Teacher;

    public ApplyRepositoryTest()
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
        Thesis cSharp = new Thesis("CSharp", 1) { Id = 5, Description = "This is a Thesis about a very interesting C# programming language" };

        context.Theses.Add(wildAlgorithms);
        context.Theses.Add(graphAlgorithms);
        context.Theses.Add(linq);
        context.Theses.Add(migration);
        context.Theses.Add(cSharp);

        Apply applies1 = new Apply(1, 1) { Id = 1 };
        Apply applies2 = new Apply(2, 1) { Id = 2 };
        Apply applies3 = new Apply(3, 1) { Id = 3 };
        Apply applies4 = new Apply(5, 1) { Id = 4 };

        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);
        context.Applies.Add(applies4);

        context.SaveChangesAsync();

        _context = context;
        _repo_apply = new ApplyRepository(_context);
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
        _repo_Teacher = new TeacherRepository(_context);
    }

    [Fact]
    public async Task ReadApplied_GivenStudentID1AndThesisID1_ReturnsResponeSuccesAndApplyDTO()
    {
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);

        var expectedDTO = new ApplyDTO(Status.Pending, student.Item2, thesis1.Item2);

        var appliedEntry = await _repo_apply.ReadApplied(1, 1);

        Assert.Equal((Response.Success, expectedDTO), appliedEntry);
    }

    [Fact]
    public async Task ReadApplied_GivenStudentID1AndThesisID5_ReturnsResponeSuccesAndApplyDTO()
    {
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(5);

        var ExpectedDTO = new ApplyDTO(Status.Pending, student.Item2, thesis1.Item2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 5);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);
    }

    [Fact]
    public async Task ReadApplied_GivenStudentID1AndThesisID2_ReturnsResponseSuccesAndApplyDTO()
    {
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        StudentDTO Alyson = new StudentDTO(1, "Alyson", "Alyson@mail.dk");
        ThesisDTO Thesis2 = new ThesisDTO(2, "GraphAlgorithms", "This is a Thesis about a very interesting algorithm", Thore);

        var ExpectedDTO = new ApplyDTO(Status.Pending, Alyson, Thesis2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 2);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);
    }

    [Fact]
    public async Task ReadApplicationsByTeacherID_GivenTeacherID1_ReturnsListOfApplyDTO()
    {
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);
        var thesis3 = await _repo_Thesis.ReadThesis(5);

        var readList = await _repo_apply.ReadApplicationsByTeacherID(teacher.Item2.Id);

        var DTO_1 = await _repo_apply.ReadApplied(student.Item2.Id, thesis1.Item2.Id);
        var DTO_2 = await _repo_apply.ReadApplied(student.Item2.Id, thesis2.Item2.Id);
        var DTO_3 = await _repo_apply.ReadApplied(student.Item2.Id, thesis3.Item2.Id);

        Assert.Collection(readList,
        thesis => Assert.Equal(DTO_1.Item2, thesis),
        thesis => Assert.Equal(DTO_2.Item2, thesis),
        thesis => Assert.Equal(DTO_3.Item2, thesis));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}