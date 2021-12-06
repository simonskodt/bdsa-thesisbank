using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext, IThesisBankContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Thesis> Theses => Set<Thesis>();
    public DbSet<Apply> Applies => Set<Apply>();

    private static string descriptionTemplate = "<p>Aliquam vestibulum morbi blandit Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Tortor consequat id porta nibh venenatis cras sed felis. Adipiscing at in tellus integer feugiat scelerisque varius morbi. Non odio euismod lacinia at quis. Risus viverra adipiscing at in tellus. Vel pretium lectus quam id leo in. <br /> Ipsum dolor sit amet consectetur adipiscing. Malesuada nunc vel risus commodo viverra maecenas accumsan lacus. Nibh tellus molestie nunc non blandit massa enim nec dui. Ut tortor pretium viverra suspendisse potenti nullam. Orci sagittis eu volutpat odio facilisis mauris sit amet. Pharetra magna ac placerat vestibulum lectus mauris. <br /> Blandit cursus risus at ultrices mi tempus imperdiet nulla. Egestas diam in arcu cursus. Ante metus dictum at tempor commodo. Mattis vulputate enim nulla aliquet porttitor lacus luctus accumsan tortor. Morbi blandit cursus risus at ultrices mi. Nam at lectus urna duis convallis convallis. Vel turpis nunc eget lorem. Quis hendrerit dolor magna eget. Libero id faucibus nisl tincidunt eget nullam.</p>";

    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options) { }


    // protected override void OnModelCreating(ModelBuilder modelBuilder) { }

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

        var ahmed = new Student("Philip Ahmed", "phhy@itu.dk");
        var leonora = new Student("Léonora Théorêt", "leonora@itu.dk");
        var alyson = new Student("Alyson D'Souza", "alyson@itu.dk");
        var victor = new Student("Victor Brorson", "victor@itu.dk");
        var simon = new Student("Simon Skødt", "simon@itu.dk");

        var thore = new Teacher("Thore", "thore@itu.dk");
        var rasmus = new Teacher("Rasmus", "rasmus@itu.dk");

        var thesis1 = new Thesis("How ITU mentally ruin students", thore);
        var thesis2 = new Thesis("Why singletons are an anti-pattern", rasmus);
        var thesis3 = new Thesis("A study on why notepad is the best IDE", thore);

        var applies1 = new Apply(thesis1, ahmed);
        var applies2 = new Apply(thesis2, leonora) { Status = Status.Denied };
        var applies3 = new Apply(thesis2, simon);
        var applies4 = new Apply(thesis2, alyson) { Status = Status.Denied };
        var applies5 = new Apply(thesis2, victor) { Status = Status.Denied };

        context.Teachers.AddRange(
           rasmus,
           thore
       );

        context.Theses.AddRange(
           thesis1,
           thesis2,
           thesis3
       );

        context.Students.AddRange(
            ahmed,
            leonora,
            alyson,
            victor,
            simon
        );

        context.Applies.AddRange(
            applies1,
            applies2,
            applies3,
            applies4,
            applies5
        );

        context.SaveChangesAsync();
    }
}