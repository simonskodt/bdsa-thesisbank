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

     public static void Seed(ThesisBankContext context)
    {
        context.Database.EnsureCreated();
        context.Database.ExecuteSqlRaw("DELETE dbo.Applies");
        context.Database.ExecuteSqlRaw("DELETE dbo.Students");
        context.Database.ExecuteSqlRaw("DELETE dbo.Teachers");
        context.Database.ExecuteSqlRaw("DELETE dbo.Theses");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Applies', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Students', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Teachers', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Theses', RESEED, 0)");

        var Ahmed = new Student("Ahmed Galal");
        var Leonora = new Student("Léonora Théorêt");
        var Alyson = new Student("Alyson D'Souza ");
        var Victor = new Student("Victor Brorson");
        var Simon = new Student("Simon Skødt");

        var Thore = new Teacher("Thore");
        var Rasmus = new Teacher("Raasmus");

        var Thesis1 = new Thesis("How ITU mentally ruin students") { Teacher = Thore};
        var Thesis2 = new Thesis("Why singletons are an anti-pattern") { Teacher = Rasmus};
        var Thesis3 = new Thesis("A study on why notepad is the best IDE") { Teacher = Thore};

        var Applies1= new Apply{
            Status = Status.Accepted,
            Thesis = Thesis1,
            Student = Ahmed
        };
        
        var Applies2= new Apply{
            Status = Status.Denied,
            Thesis = Thesis2,
            Student = Leonora
        };

        var Applies3= new Apply{
            Status = Status.Pending,
            Thesis = Thesis2,
            Student = Simon
        };

        var Applies4= new Apply{
            Status = Status.Denied,
            Thesis = Thesis2,
            Student = Alyson
        };

        var Applies5= new Apply{
            Status = Status.Denied,
            Thesis = Thesis2,
            Student = Victor
        };

         context.Teachers.AddRange(
            Rasmus,
            Thore
        ); 

        context.SaveChanges();

         context.Theses.AddRange(
            Thesis1,
            Thesis2,
            Thesis3
        ); 

        context.Students.AddRange(
            Ahmed,
            Leonora,
            Alyson,
            Victor,
            Simon
        );
         context.SaveChanges();

        context.Applies.AddRange(
            Applies1,
            Applies2,
            Applies3,
            Applies4,
            Applies5
        );
 
        context.SaveChanges();
    }
}