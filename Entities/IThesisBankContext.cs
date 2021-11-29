namespace Entities;

public interface IThesisBankContext : IDisposable
{
    public DbSet<Student>? Students { get; set; }
    public DbSet<Teacher>? Teachers { get; set; }
    public DbSet<Thesis>? Theses { get; set; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}