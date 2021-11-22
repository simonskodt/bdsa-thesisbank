
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
    public void test(){

    }

    public void Dispose(){

        _context.Dispose();

    }
}
