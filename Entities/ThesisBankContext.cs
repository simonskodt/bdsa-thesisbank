using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext, IThesisBankContext
{
public DbSet<Student>? Students { get; set; }
public DbSet<Teacher>? Teachers { get; set; }
public DbSet<Thesis>? Theses { get; set; }

public DbSet<Apply>? Applies {get ; set;}

    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options) { }

/*         protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }   */
}