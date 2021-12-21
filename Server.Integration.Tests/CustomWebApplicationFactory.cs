namespace Server.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ThesisBankContext>));

            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            /* Overriding policies and adding Test Scheme defined in TestAuthHandler */
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Test")
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            var connection = new SqliteConnection("Filename=:memory:");

            services.AddDbContext<ThesisBankContext>(options =>
            {
                options.UseSqlite(connection);
            });

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ThesisBankContext>();
            appContext.Database.OpenConnection();
            appContext.Database.EnsureCreated();

            Seed(appContext);
        });

        builder.UseEnvironment("Integration");

        return base.CreateHost(builder);
    }

    private void Seed(ThesisBankContext context)
    {
        var ahmed = new Student("Philip Ahmed", "phhy@itu.dk");
        var leonora = new Student("Léonora Théorêt", "leonora@itu.dk");
        var alyson = new Student("Alyson D'Souza", "alyson@itu.dk");
        var victor = new Student("Victor Brorson", "victor@itu.dk");
        var simon = new Student("Simon Skødt", "simon@itu.dk");

        var thore = new Teacher("Thore", "thore@itu.dk");
        var rasmus = new Teacher("Rasmus", "rasmus@itu.dk");

        var thesis1 = new Thesis("How ITU mentally ruin students", thore);
        thesis1.Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        var thesis2 = new Thesis("Why singletons are an anti-pattern", rasmus);
        thesis2.Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        var thesis3 = new Thesis("A study on why notepad is the best IDE", thore);

        var applies1 = new Apply(thesis1, ahmed);
        var applies2 = new Apply(thesis2, leonora); 
        var applies3 = new Apply(thesis2, simon);
        var applies4 = new Apply(thesis2, alyson);
        var applies5 = new Apply(thesis2, victor);

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

        context.SaveChanges();
    }
}