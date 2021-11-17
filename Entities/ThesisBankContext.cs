namespace Entities;
public class ThesisBankContext : DbContext 
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Thesis> Theses { get; set; }

    public ThesisBankContext(){}
    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}