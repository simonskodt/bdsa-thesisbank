using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext, IThesisBankContext
{
public DbSet<Student>? Students { get; set; }
public DbSet<Teacher>? Teachers { get; set; }
public DbSet<Thesis>? Theses { get; set; }

public DbSet<Apply>? Applies {get ; set;}

private static string descriptionTemplate = "<p>Aliquam vestibulum morbi blandit Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Tortor consequat id porta nibh venenatis cras sed felis. Adipiscing at in tellus integer feugiat scelerisque varius morbi. Non odio euismod lacinia at quis. Risus viverra adipiscing at in tellus. Vel pretium lectus quam id leo in. <br /> Ipsum dolor sit amet consectetur adipiscing. Malesuada nunc vel risus commodo viverra maecenas accumsan lacus. Nibh tellus molestie nunc non blandit massa enim nec dui. Ut tortor pretium viverra suspendisse potenti nullam. Orci sagittis eu volutpat odio facilisis mauris sit amet. Pharetra magna ac placerat vestibulum lectus mauris. <br /> Blandit cursus risus at ultrices mi tempus imperdiet nulla. Egestas diam in arcu cursus. Ante metus dictum at tempor commodo. Mattis vulputate enim nulla aliquet porttitor lacus luctus accumsan tortor. Morbi blandit cursus risus at ultrices mi. Nam at lectus urna duis convallis convallis. Vel turpis nunc eget lorem. Quis hendrerit dolor magna eget. Libero id faucibus nisl tincidunt eget nullam.</p>";

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

        var Philip = new Student("Philip Hyltoft");
        var Leonora = new Student("Léonora Théorêt");
        var Alyson = new Student("Alyson D'Souza ");
        var Victor = new Student("Victor Brorson");
        var Simon = new Student("Simon Skødt");

        var Thore = new Teacher("Thore");
        var Rasmus = new Teacher("Rasmus");

        var Thesis1 = new Thesis("How ITU mentally ruin students") { Teacher = Thore, Description = descriptionTemplate};
        var Thesis2 = new Thesis("Why singletons are an anti-pattern") { Teacher = Rasmus, Description = descriptionTemplate};
        var Thesis3 = new Thesis("A study on why notepad is the best IDE") { Teacher = Thore, Description = descriptionTemplate};

        var Applies1= new Apply{
            Status = Status.Accepted,
            Thesis = Thesis1,
            Student = Philip
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
            Philip,
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