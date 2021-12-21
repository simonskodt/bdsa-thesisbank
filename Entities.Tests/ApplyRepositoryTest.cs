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
        Student victor = new Student("Victor", "victor@mail.dk") { Id = 2 };
        Student leonora = new Student("Leonora", "leonora@itu.dk") { Id = 3 };

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
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis1_ReturnResonseSuccessAndAppliedDTO()
    {
        var expectedApplied = new ApplyDTOids(5, Status.Pending, 1, 1);

        var readApplied = await _repo_apply.ApplyForThesis(1, 1);

        Assert.Equal((Response.Created, expectedApplied), readApplied);
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent9AndThesis1_ReturnResonseNotFoundAndNull()
    {
        var readApplied = await _repo_apply.ApplyForThesis(9, 1);

        Assert.Equal((Response.NotFound, null), readApplied);
    }


    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis9_ReturnResonseNotFoundAndNull()
    {
        var readApplied = await _repo_apply.ApplyForThesis(1, 9);

        Assert.Equal((Response.NotFound, null), readApplied);
    }

    // to check that it is posible for two different students to apply for the same thesis
    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent2AndThesis1_ReturnResonseSuccessAndAppliedDTO()
    {
        var expectedApplied = new ApplyDTOids(5, Status.Pending, 2, 1);

        var readApplied = await _repo_apply.ApplyForThesis(2, 1);

        Assert.Equal((Response.Created, expectedApplied), readApplied);
    }


    [Fact]
    public async Task RemoveRequest_Givenapplyid1_Returndeleted()
    {
        var readRemoved = await _repo_apply.DeleteApplied(1);

        Assert.Equal(Response.Deleted, readRemoved);
    }


    [Fact]
    public async Task ReadAppliedByStudentID_GivenID1_returnsAppliesID_1_2_3_4(){
        
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);
        var thesis3 = await _repo_Thesis.ReadThesis(3);
        var thesis5 = await _repo_Thesis.ReadThesis(5);

        var readList = await _repo_apply.ReadAppliedByStudentID(student.Item2.Id);

        var expectedList = new List<ApplyDTOWithMinalThesis>();        
            var apply1 = new ApplyDTOWithMinalThesis(1, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis1.Item2.Id, thesis1.Item2.Name, null, thesis1.Item2.Teacher.Name));
            var apply2 = new ApplyDTOWithMinalThesis(2, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis2.Item2.Id, thesis2.Item2.Name, null, thesis2.Item2.Teacher.Name));
            var apply3 = new ApplyDTOWithMinalThesis(3, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis3.Item2.Id, thesis3.Item2.Name, null, thesis3.Item2.Teacher.Name));
            var apply4 = new ApplyDTOWithMinalThesis(4, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis5.Item2.Id, thesis5.Item2.Name, null, thesis5.Item2.Teacher.Name));

        expectedList.Add(apply1);
        expectedList.Add(apply2);
        expectedList.Add(apply3);
        expectedList.Add(apply4);

        Assert.Equal(readList, expectedList);

    }

    [Fact]
    public async Task ReadAppliedByStudentID_GivenID1_ReturnsSameAppliesAs_ReadApplied(){
        var ReadAppliedByStudentIDList = await _repo_apply.ReadAppliedByStudentID(1);
        var ReadAppliedList = await _repo_apply.ReadApplied();

        var newList = new List<ApplyDTOids>();
        foreach (var apply in ReadAppliedByStudentIDList)
        {
            var applyDTOid = new ApplyDTOids(apply.Id, apply.Status, apply.Student.Id, apply.Thesis.Id);
            newList.Add(applyDTOid);

        }
        Assert.Equal(ReadAppliedList, newList);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task ReadAppliedByStudentID_GivenID1_returnsAppliesID_1_2_3_4(){
        
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);
        var thesis3 = await _repo_Thesis.ReadThesis(3);
        var thesis5 = await _repo_Thesis.ReadThesis(5);

        var readList = await _repo_apply.ReadAppliedByStudentID(student.Item2.Id);

        var expectedList = new List<ApplyDTOWithMinalThesis>();        
            var apply1 = new ApplyDTOWithMinalThesis(1, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis1.Item2.Id, thesis1.Item2.Name, null, thesis1.Item2.Teacher.Name));
            var apply2 = new ApplyDTOWithMinalThesis(2, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis2.Item2.Id, thesis2.Item2.Name, null, thesis2.Item2.Teacher.Name));
            var apply3 = new ApplyDTOWithMinalThesis(3, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis3.Item2.Id, thesis3.Item2.Name, null, thesis3.Item2.Teacher.Name));
            var apply4 = new ApplyDTOWithMinalThesis(4, Status.Pending, student.Item2,
            new ThesisDTOMinimal(thesis5.Item2.Id, thesis5.Item2.Name, null, thesis5.Item2.Teacher.Name));

        expectedList.Add(apply1);
        expectedList.Add(apply2);
        expectedList.Add(apply3);
        expectedList.Add(apply4);

        Assert.Equal(readList, expectedList);

    }

    [Fact]
    public async Task ReadAppliedByStudentID_GivenID1_ReturnsSameAppliesAs_ReadApplied(){
        var ReadAppliedByStudentIDList = await _repo_apply.ReadAppliedByStudentID(1);
        var ReadAppliedList = await _repo_apply.ReadApplied();

        var newList = new List<ApplyDTOids>();
        foreach (var apply in ReadAppliedByStudentIDList)
        {
            var applyDTOid = new ApplyDTOids(apply.Id, apply.Status, apply.Student.Id, apply.Thesis.Id);
            newList.Add(applyDTOid);

        }
        Assert.Equal(ReadAppliedList, newList);
    }
}