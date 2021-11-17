using Entities;
using ThesisBank.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

static IConfiguration LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>();

        return builder.Build();
    }

    static void Main(string[] args)
    {
        var configuration = LoadConfiguration();
        //var connectionString = "Server=localhost;Database=MyProject;User Id=sa;Password=37d6661e-6894-4944-88fc-e07256e30c81";
        //var connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"; //(retrieve password)
        var connectionString = configuration.GetConnectionString("ThesisBank");
        //using var connection = new SqlConnection(connectionString);
        var optionsBuilder = new DbContextOptionsBuilder<ThesisBankContext>().UseSqlServer(connectionString);
        //var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseNpgsql(connectionString); //POSTGRES
        using var context = new ThesisBankContext(optionsBuilder.Options);
        
    }