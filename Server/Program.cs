




var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAd", options);
            options.TokenValidationParameters.RoleClaimType =
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        },
        options =>
        {
            builder.Configuration.Bind("AzureAd", options);
        });

builder.Services.Configure<JwtBearerOptions>(
    JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters.NameClaimType = "name";
    });

builder.Services.AddRazorPages();

//using var context = new ThesisBankContext(optionsBuilder.Options);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
    c.UseInlineDefinitionsForEnums();
});

var connectionString = builder.Configuration.GetConnectionString("ThesisBank");
builder.Services.AddDbContext<ThesisBankContext>(options=>options.UseSqlServer(connectionString,options => options.EnableRetryOnFailure(5)));
//builder.Services.AddDbContext<ThesisBankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ThesisBank")));
builder.Services.AddScoped<IThesisBankContext, ThesisBankContext>();
builder.Services.AddScoped<IThesisRepository, ThesisRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IApplyRepository, ApplyRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//var configuration = LoadConfiguration();

//ThesisBankContext.Seed(context); //Seed Extension IHost repo SeedExtensions


/*



static IConfiguration LoadConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddUserSecrets<Program>();

    return builder.Build();
}
*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.SeedAsync();

app.Run();

public partial class Program { }