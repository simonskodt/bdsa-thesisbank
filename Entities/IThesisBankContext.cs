namespace Entities;

public interface IThesisBankContext : IDisposable
{
    public DbSet<Student> Students { get; }
    public DbSet<Teacher> Teachers { get; }
    public DbSet<Thesis> Theses { get; }
    public DbSet<Apply> Applies { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}