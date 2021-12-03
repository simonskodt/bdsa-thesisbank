namespace Entities.Tests;
public class ApplyRepositoryTest : IDisposable
{
    readonly ThesisBankContext _context;
    private readonly ApplyRepository? _repo_apply;

    private readonly StudentRepository? _repo_Stud;
    private readonly ThesisRepository? _repo_Thesis;

    private readonly TeacherRepository? _repo_Teacher;


    public ApplyRepositoryTest()
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
        WildAlgorithms.Description = "This is a Thesis about a very interesting C# programming language";

        context.Theses.Add(WildAlgorithms);
        context.Theses.Add(GraphAlgorithms);
        context.Theses.Add(Linq);
        context.Theses.Add(Migration);
        context.Theses.Add(CSharp);

        
        Apply applies1 = new Apply{Id =1, Status = Status.Pending, ThesisID = 1, StudentID = 1};
        Apply applies2 = new Apply{Id =2, Status = Status.Pending, ThesisID = 2, StudentID = 1}; 
        Apply applies3 = new Apply{Id =3, Status = Status.Pending, ThesisID = 3, StudentID = 1};
        Apply applies4 = new Apply{Id =4, Status = Status.Pending, ThesisID = 5, StudentID = 1};
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
    public async Task ReadApplied_GivenStudentID1AndThesisID1_ReturnsResponeSuccesAndApplyDTO(){
        
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        
        var ExpectedDTO = new ApplyDTO(Status.Pending, student.Item2, thesis1.Item2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 1);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }
        
        [Fact]
        public async Task ReadApplied_GivenStudentID1AndThesisID5_ReturnsResponeSuccesAndApplyDTO(){
        
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(5);
        
        var ExpectedDTO = new ApplyDTO(Status.Pending, student.Item2, thesis1.Item2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 5);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }

    [Fact]
    public async Task ReadApplied_GivenStudentID1AndThesisID2_ReturnsResponseSuccesAndApplyDTO(){
        
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        StudentDTO Alyson = new StudentDTO{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        ThesisDTO Thesis2 = new ThesisDTO(2, "GraphAlgorithms", "This is a Thesis about a very interesting algorithm", Thore);

        var ExpectedDTO = new ApplyDTO(Status.Pending, Alyson, Thesis2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 2);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }
    [Fact]
    public async Task ReadApplicationsByTeacherID_GivenTeacherID1_ReturnsListOfApplyDTO(){

        //Arrange
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);
        var thesis3 = await _repo_Thesis.ReadThesis(5);


        var readList = await _repo_apply.ReadApplicationsByTeacherID(teacher.Item2.Id);


        var DTO_1 = await _repo_apply.ReadApplied(student.Item2.Id, thesis1.Item2.Id);
        var DTO_2 = await _repo_apply.ReadApplied(student.Item2.Id, thesis2.Item2.Id);
        var DTO_3 = await _repo_apply.ReadApplied(student.Item2.Id, thesis3.Item2.Id);

        //Assert
        Assert.Collection(readList,
        thesis => Assert.Equal(DTO_1.Item2, thesis),
        thesis => Assert.Equal(DTO_2.Item2, thesis),
        thesis => Assert.Equal(DTO_3.Item2, thesis));
    }
    
    public void Dispose(){

        _context.Dispose();

    }


}