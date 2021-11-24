
namespace Entities.Tests;

public class TeacherRepositoryTest : IDisposable
{
    private readonly ThesisBankContext _context;
    private readonly TeacherRepository _repo;

    public TeacherRepositoryTest(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ThesisBankContext>();
        builder.UseSqlite(connection);
        var context = new ThesisBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.SaveChanges();



        _context = context;
        _repo = new TeacherRepository(_context);
    }

    [Fact]
    public void Accept_Thesis_given_ThesisID()
    {
        // public record TeacherDTO(int Id, string Name, string Email);
        TeacherDTO 
    }

    public void Dispose(){

        _context.Dispose();

    }
}
