using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


using Entities;

public class ThesisBankContextFactory : IDesignTimeDbContextFactory<ThesisBankContext>
{
    public ThesisBankContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("ThesisBank");

        var optionsBuilder = new DbContextOptionsBuilder<ThesisBankContext>()
            .UseSqlServer(connectionString);

        return new ThesisBankContext(optionsBuilder.Options);
    }
}