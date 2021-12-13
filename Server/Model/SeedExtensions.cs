namespace Server.Model;
using Entities;
using Microsoft.EntityFrameworkCore;
using Core;

public static class SeedExtensions
{
    public static async Task<IHost> SeedAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ThesisBankContext>();
            await Seed(context);
        }
        return host;
    }

    private static async Task Seed(ThesisBankContext context)
    {
        await context.Database.MigrateAsync();

        if(!await context.Students.AnyAsync() && !await context.Teachers.AnyAsync() && !await context.Theses.AnyAsync() && !await context.Applies.AnyAsync()){
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
        var Alyson = new Student("Alyson De Souza", "Alyson@itu.dk");
        var Victor = new Student("Viggo", "Victor@itu.dk");
        var Simon = new Student("Simon Johann Skødt", "Simon@itu.dk");

        var Leo = new Teacher("Leo", "Leonora@itu.dk");
        var Rasmus = new Teacher("Rasmus", "Rasmus@itu.dk");

        var Thesis1 = new Thesis("Why algorithms are brilliant", Leo);
        var Thesis2 = new Thesis("Why singletons are an anti-pattern", Rasmus);
        var Thesis3 = new Thesis("A study on why notepad is the best IDE", Leo);

        var Applies1 = new Apply(Thesis1, Ahmed);
        var Applies2 = new Apply(Thesis2, Simon);
        var Applies3 = new Apply(Thesis2, Alyson) { Status = Status.Denied };
        var Applies4 = new Apply(Thesis2, Victor) { Status = Status.Denied };

        context.Teachers.AddRange(
           Rasmus,
           Leo
       );

        await context.SaveChangesAsync();

        context.Theses.AddRange(
           Thesis1,
           Thesis2,
           Thesis3
       );

        await context.SaveChangesAsync();

        context.Students.AddRange(
            Ahmed,
            Alyson,
            Victor,
            Simon
        );

        await context.SaveChangesAsync();
        
        context.Applies.AddRange(
            Applies1,
            Applies2,
            Applies3,
            Applies4
        );

        await context.SaveChangesAsync();
        }
    }
}