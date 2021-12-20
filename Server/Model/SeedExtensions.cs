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

        var ahmed = new Student("Ahmed Galal", "Ahmed@itu.dk");
        var alyson = new Student("Alyson De Souza", "Alyson@itu.dk");
        var victor = new Student("Viggo", "Victor@itu.dk");
        var simon = new Student("Simon Johann Skødt", "Simon@itu.dk");
        var leonora = new Student("Leo", "Leo@itu.dk");
        var philip = new Student("Philip", "Philip@itu.dk");
        var peter = new Student("Peter SWU", "StudentPeterSWU@hotmail.com");



        var albert = new Teacher("Albert Jones", "TeacherAlbertJones@hotmail.com");
        var anne = new Teacher("Anne Collins", "TeacherAnneCollins@hotmail.com");

        var discription = "Felis donec et odio pellentesque diam volutpat commodo. A scelerisque purus semper eget duis at tellus. Aliquet enim tortor at auctor urna nunc id cursus metus. Bibendum neque egestas congue quisque egestas. Hac habitasse eles ut. Ut lectus arcu bibendum at varius. Ac turpis egestas integer eget. Fusce id velit ut tortor pretium. Facilisi etiam dignissim diam quis. Interdum velit euismod in pellentesque massa placerat duis. Ornare lectus sit amet est placerat. Nibh ipsum consequat nisl vel pretium lectus quam. Congue nisi vitae suscipit tellus mauris a diam maecenas. Consectetur a erat nam at lectus. Tincidunt nunc pulvinar sapien et. Quis viverra nibh cras pulvinar mattis nunc sed. Fringilla est ullamcorper eget nulla facilisi etiam dignissim. Ornare arcu dui vivamus arcu felis bibendum ut tristique et. Viverra orci sagittis eu volutpat odio facilisis mauris. Nunc non blandit massa enim nec dui. Nec feugiat in fermentum posuere urna nec tincidunt praesent. In aliquam sem fringilla ut morbi tincidunt augue. Eget felis eget nunc lobortis mattis. Vestibulum mattis ullamcorper velit sed ullamcorper. Tincidunt ornare massa eget egestas purus. Consequat mauris nunc congue nisi vitae suscipit tellus. Praesent semper feugiat nibh sed. Mi in nulla posuere sollicitudin aliquam.";

        var thesis1 = new Thesis("Why Algorithms are Brilliant", albert){Description = discription, Excerpt = "The main focus in this thesis lies on Dijkstra algorithm and other algorithm used frequently"};
        var thesis2 = new Thesis("Why Singletons are an Anti-pattern", anne){Description = discription, Excerpt = "A thesis covering the myth of Singeltons "};
        var thesis3 = new Thesis("A Study on why Notepad is the Best IDE", anne){Description = discription, Excerpt = "So many IDE is out in the industry today, so to be aware of the superiority notepad possess is important"};
        var thesis4 = new Thesis("Complementary Contrasts in UI", anne){Description = discription, Excerpt = "Do not under estimates the impact of Complementary contrasts, this thesis will be a eye opener of colors"};
        var thesis5 = new Thesis("Why Clean Onion is the Best Architecture", albert){Description = discription, Excerpt = "To find the best architecture to you system is always difficult, but this will be change, be prepared for clean Onion"};
        var thesis6 = new Thesis("Why HTML is Usefull", albert){Description = discription, Excerpt = "All about HTML and a little view into CSS. HTML is the fodation of all, so this thesis is for students that have a purpose"};
        var thesis7 = new Thesis("Pros and Cons of Object Oriented Programming", albert){Description = discription, Excerpt = "Object Oriented Programming is simulation the real world, this is nice when programming. To get a deep understandig of the pros of Object Oriented Programming choose this thesis"};
        var thesis8 = new Thesis("A Study of why Math is Important in Programmimg", albert){Description = discription, Excerpt = "Programming is a language, but a good fundation is to have a mathematical understandig. Math should not be underestimated"};
        var thesis9 = new Thesis("A Comparison of Java and C#", albert){Description =  discription, Excerpt = "Clases, interfaces, records, methodes etc. A deep understandig of the two languages and where to use which"};
        var thesis10 = new Thesis("Golang and Distributed Systems", albert){Description = discription, Excerpt = "Goland, CAP, RAFT etc. This Thesis is a dream for students that liked DISYS"};
        var thesis11 = new Thesis("The Magic of SQL", albert){Description = discription, Excerpt = "SELECT this thises\nFROM Theses\nWHERE title = 'The magic of SQL'"};

        context.Teachers.AddRange(
           anne,
           albert
       );

        await context.SaveChangesAsync();

        context.Theses.AddRange(
           thesis1,
           thesis2,
           thesis3,
           thesis4,
           thesis5,
           thesis6,
           thesis7,
           thesis8,
           thesis9,
           thesis10,
           thesis11
       );

        await context.SaveChangesAsync();

        context.Students.AddRange(
            ahmed,
            alyson,
            victor,
            simon,
            leonora,
            philip,
            peter
        );
        await context.SaveChangesAsync();
        }
    }
}