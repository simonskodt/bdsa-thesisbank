namespace Entities.Tests;

public class StudentRepositoryTest : IDisposable
{
    private readonly ThesisBankContext? _context;
    private readonly StudentRepository? _repo_Stud;
    private readonly ThesisRepository? _repo_Thesis;


    public StudentRepositoryTest(){

        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();

        Teacher Thore = new Teacher("Thore");
        Thore.Email = "Thore@itu.dk";
        Thore.Id = 1;
        context.Teachers.Add(Thore);


        Student Alyson = new Student("Alyson");
        Alyson.Email = "Alyson@mail.dk";
        Alyson.Id = 1;

        Student Victor = new Student("Victor");
        Victor.Email = "Victor@mail.dk";
        Victor.Id = 2;

        Student Leonora = new Student("Leonora");
        Leonora.Email = "Leonora@itu.dk";
        Leonora.Id = 3;

        context.Students.Add(Alyson);
        context.Students.Add(Victor);
        context.Students.Add(Leonora);


        Thesis WildAlgorithms = new Thesis("WildAlgorithms", Thore);
        WildAlgorithms.Id = 1;
        WildAlgorithms.Description = "This is a Thesis about a very interesting topic";

        Thesis GraphAlgorithms = new Thesis("GraphAlgorithms", Thore);
        WildAlgorithms.Id = 2;
        WildAlgorithms.Description = "This is a Thesis about a very interesting algorithm";

        Thesis designingUI = new Thesis("designingUI", Thore);
        WildAlgorithms.Id = 3;
       
        context.Theses.Add(WildAlgorithms);
        context.Theses.Add(GraphAlgorithms);
        context.Theses.Add(designingUI);


        context.Applies.Add(new Apply { Id = 1, Status = Status.Pending, ThesisID = 1, StudentID = 1});
        context.Applies.Add(new Apply { Id = 2, Status = Status.Accepted, ThesisID = 2, StudentID = 2});
        context.Applies.Add(new Apply { Id = 3, Status = Status.Denied, ThesisID = 1, StudentID = 2});
        context.Applies.Add(new Apply { Id = 4, Status = Status.Archived, ThesisID = 2, StudentID = 1});
        
        context.Applies.Add(new Apply { Id = 5, Status = Status.Pending, ThesisID = 1, StudentID = 3});
        context.Applies.Add(new Apply { Id = 6, Status = Status.Pending, ThesisID = 2, StudentID = 3});
        context.Applies.Add(new Apply { Id = 7, Status = Status.Pending, ThesisID = 3, StudentID = 3});



        context.SaveChangesAsync();


        _context = context;
        _repo_Stud = new StudentRepository(_context);
        _repo_Thesis = new ThesisRepository(_context);
    }


    /*

    TESTS FOR METHODE ReadStudent

    */

    [Fact]
    public async Task ReadStudent_GivenStudent1_ReturnResonseSuccessAndStudent1DTO(){
        
        var actual = await _repo_Stud.ReadStudent(1);

        var expectedStudentDTO = new StudentDTO{Id = 1, Name = "Victor", Email = "Vibr@itu.dk"};

        Assert.Equal((Response.Success, expectedStudentDTO), actual);
    }

    [Fact]
    public async Task ReadStudent_GivenStudent9_ReturnResonseNotFoundAndNull(){
        
        var actual = await _repo_Stud.ReadStudent(9);

        Assert.Equal((Response.NotFound, null), actual);
    }

    /*

    TESTS FOR METHODE ApplyForThesis

    */

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis1_ReturnResonseSuccessAndAppliedDTO(){
        
        var student = await _repo_Stud.ReadStudent(1);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(1, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
       
    }

    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent9AndThesis1_ReturnResonseNotFoundAndNull(){

        var readApplied = await _repo_Stud.ApplyForThesis(9, 1);

        Assert.Equal((Response.NotFound, null), readApplied);
    }


    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent1AndThesis9_ReturnResonseNotFoundAndNull(){

        var readApplied = await _repo_Stud.ApplyForThesis(1, 9);

        Assert.Equal((Response.NotFound, null), readApplied);
    }

    // to check that it is posible for two different students to apply for the same thesis
    [Fact]
    public async Task ApplyForThesis_GivenAppliedStudent2AndThesis1_ReturnResonseSuccessAndAppliedDTO(){
        
        var student = await _repo_Stud.ReadStudent(2);
        var thesis = await _repo_Thesis.ReadThesis(1);
        var expectedApplied = new ApplyDTO(Status.Pending, student.Item2, thesis.Item2);

        var readApplied = await _repo_Stud.ApplyForThesis(2, 1);

        Assert.Equal((Response.Success, expectedApplied), readApplied);
    }


    /*

    TESTS FOR METHODE Accept

    */

    [Theory]
    [InlineData(false, Response.Updated, Status.Archived, 2, 2)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    [InlineData(true, Response.NotFound, Status.Pending, 1, 1)]
    public async Task Accept_GivenStudentThesis_ReturnStatus(Boolean isNullDTO, Response expectedResponse, Status expectedStatus, int expectedStudentID, int expectedThesisID){
        
        var student = await _repo_Stud.ReadStudent(expectedStudentID);
        var thesis = await _repo_Thesis.ReadThesis(expectedThesisID);

        ApplyDTO? expectedApplied;
        if(isNullDTO){
            expectedApplied = null;
        } else {
           expectedApplied = new ApplyDTO(expectedStatus, student.Item2, thesis.Item2);
        }

        var readApplied = await _repo_Stud.Accept(expectedStudentID, expectedThesisID);

        
        Assert.Equal((expectedResponse, expectedApplied), readApplied);
    }

    /*

    TESTS FOR METHODE RemoveAllPendings

    */


    [Fact]
    public async Task RemoveAllPendings_GivenStudent1_ReturnsDeleted(){
        
        var readResponse = await _repo_Stud.RemoveAllPendings(1);

        Assert.Equal(Response.Deleted, readResponse);
        //Assert.Empty((await _repo_Thesis.ReadPendingThesis(1)));
        
    }

    [Fact]
    public async Task RemoveAllPendings_GivenStudentId3_ReturnDeleted(){

        var readAllRemoved = await _repo_Stud.RemoveAllPendings(3);

        Assert.Equal(Response.Deleted, readAllRemoved);
        Assert.Empty((await _repo_Thesis.ReadPendingThesis(3)));
    }

    /*

    TESTS FOR METHODE RemoveRequest

    */

    [Fact]
    public async Task RemoveRequest_GivenThesis1_ReturnResponseSuccessAndThesisDTOWithThesis1(){

        var readRemoved = await _repo_Stud.RemoveRequest(1,1);

        Assert.Equal(Response.Deleted, readRemoved);
        //Assert.Empty((await _repo_Thesis.ReadPendingThesis(1)));
    }

    public void Dispose(){

        _context.Dispose();

    }
}