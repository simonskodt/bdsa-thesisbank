using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext
{
public DbSet<Student>? Students { get; set; }
public DbSet<Teacher>? Teachers { get; set; }
public DbSet<Thesis>? Theses { get; set; }

public DbSet<Applies>? Applies {get ; set;}

    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Applies>()
            .HasMany<Student>(s => s.appliedStudents);
        
        modelBuilder.Entity<Applies>()
            .HasMany<Thesis>( t => t.appliedTheses);*/

        modelBuilder.Entity<Applies>().HasKey( a => new {a.appliedStudents, a.appliedTheses});

        modelBuilder.Entity<Student>().Property(s => s.Id).IsRequired();
        modelBuilder.Entity<Thesis>().Property(t => t.Id).IsRequired();
        modelBuilder.Entity<Thesis>().Property(t => t.teacher).IsRequired();
        base.OnModelCreating(modelBuilder);
    } 
}