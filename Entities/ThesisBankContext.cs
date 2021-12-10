
﻿﻿using System;
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

    /*
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

        var Ahmed = new Student("Ahmed Galal", "Ahmed@itu.dk");
        var Leonora = new Student("Léonora Théorêt", "Leonora@itu.dk");
        var Alyson = new Student("Alyson D'Souza", "Alyson@itu.dk");
        var Victor = new Student("Victor Brorson", "Victor@itu.dk");
        var Simon = new Student("Simon Skødt", "Simon@itu.dk");

        var Thore = new Teacher("Thore", "Thore@itu.dk");
        var Rasmus = new Teacher("Rasmus", "Rasmus@itu.dk");

        var Thesis1 = new Thesis("How ITU mentally ruin students", Thore);
        var Thesis2 = new Thesis("Why singletons are an anti-pattern", Rasmus);
        var Thesis3 = new Thesis("A study on why notepad is the best IDE", Thore);

        var Applies1 = new Apply(Thesis1, Ahmed);
        var Applies2 = new Apply(Thesis2, Leonora) { Status = Status.Denied };
        var Applies3 = new Apply(Thesis2, Simon);
        var Applies4 = new Apply(Thesis2, Alyson) { Status = Status.Denied };
        var Applies5 = new Apply(Thesis2, Victor) { Status = Status.Denied };

        context.Teachers.AddRange(
           Rasmus,
           Thore
       );

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

        context.Applies.AddRange(
            Applies1,
            Applies2,
            Applies3,
            Applies4,
            Applies5
        );

        context.SaveChangesAsync();
    }
    */
    }
