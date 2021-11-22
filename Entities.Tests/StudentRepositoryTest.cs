
namespace Entities.Tests;

public class UnitTest1
{
    public class StudentRepositoryTest : IDisposable
    {
        private readonly ThesisBankContext _context;
        private readonly StudentRepository _repo;

        public StudentRepositoryTest(){

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<ThesisBankContext>();
            builder.UseSqlite(connection);
            var context = new ThesisBankContext(builder.Options);
            context.Database.EnsureCreated();
            context.SaveChanges();

            _context = context;
            _repo = new StudentRepository(_context);
        }
    

        [Fact]
        public void test(){

        }

        public void Dispose(){

            _context.Dispose();

        }
    }
}