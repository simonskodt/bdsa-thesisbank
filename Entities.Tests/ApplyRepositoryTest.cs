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

        Teacher Thore = new Teacher {Id = 1, Name = "Thore", Email = "Thore@itu.dk"};
        Teacher Rasmus = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};
        context.Teachers.Add(Thore);
        context.Teachers.Add(Rasmus);

        Student Alyson = new Student{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        Student Victor = new Student{Id = 2, Name = "Victor", Email ="Victor@mail.dk"};
        context.Students.Add(Alyson);
        context.Students.Add(Victor);

        context.Theses.Add(new Thesis { Id = 1, Name = "WildAlgorithms", Description ="This is a Thesis about a very interesting topic", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 2, Name = "GraphAlgorithms", Description ="This is a Thesis about a very interesting algorithm", Teacher = Thore });
        context.Theses.Add(new Thesis { Id = 3, Name = "Linq", Description ="This is a Thesis about a very interesting linq", Teacher = Rasmus });
        context.Theses.Add(new Thesis { Id = 4, Name = "Migration",Description ="This is a Thesis about a very interesting Migration", Teacher = Rasmus });

        
        Apply applies1 = new Apply{Id =1, Status = Status.Pending, ThesisID = 1, StudentID = 1};
        Apply applies2 = new Apply{Id =2, Status = Status.Pending, ThesisID = 2, StudentID = 1}; 
        Apply applies3 = new Apply{Id =3, Status = Status.Pending, ThesisID = 3, StudentID = 1}; 
        context.Applies.Add(applies1);
        context.Applies.Add(applies2);
        context.Applies.Add(applies3);
        context.SaveChangesAsync();

        _context = context;
        _repo_apply = new ApplyRepository(_context);
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
        _repo_Teacher = new TeacherRepository(_context);

    }
    [Fact]
    public async Task Given_Student_ID_1_and_Thesis_Id_1_Returns_Applied_id_1(){

        
        
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        StudentDTO Alyson = new StudentDTO{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        ThesisDTO Thesis1 = new ThesisDTO(1, "WildAlgorithms", "This is a Thesis about a very interesting topic", Thore);
        
        var ExpectedDTO = new ApplyDTO(Status.Pending, Alyson, Thesis1);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 1);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }

    [Fact]
    public async Task chih(){
        
        TeacherDTO Thore = new TeacherDTO(1, "Thore", "Thore@itu.dk");
        StudentDTO Alyson = new StudentDTO{Id = 1, Name = "Alyson", Email ="Alyson@mail.dk"};
        ThesisDTO Thesis2 = new ThesisDTO(2, "GraphAlgorithms", "This is a Thesis about a very interesting algorithm", Thore);

        var ExpectedDTO = new ApplyDTO(Status.Pending, Alyson, Thesis2);

        var AppliedEntry = await _repo_apply.ReadApplied(1, 2);

        Assert.Equal((Response.Success, ExpectedDTO), AppliedEntry);

    }

    [Fact]
    public async Task Given_Thore_Return_List_Of_ApplyDTO_With_Thesis_ID_1_And_2(){

        //Arrange
        var teacher = await _repo_Teacher.ReadTeacher(1);
        var student = await _repo_Stud.ReadStudent(1);
        var thesis1 = await _repo_Thesis.ReadThesis(1);
        var thesis2 = await _repo_Thesis.ReadThesis(2);

        var readList = await _repo_apply.ReadApplicationsByTeacherID(1);

        var DTO_2 = await _repo_apply.ReadApplied(student.Item2.Id, thesis2.Item2.Id);
        var DTO_1 = await _repo_apply.ReadApplied(student.Item2.Id, thesis1.Item2.Id);
       


        //Assert
 Assert.Collection(readList,
thesis => Assert.Equal(DTO_1.Item2, thesis),
thesis => Assert.Equal(DTO_2.Item2, thesis));


    }
    
    public void Dispose(){

        _context.Dispose();

    }


}