/* namespace Server.Integration.Tests;

// Credit to Rasmus Lystrøm for providing an integration test structure, which is used in this directory

public class ThesisTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ThesisTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task Get_Returns_theses()
    {
        var theses = await _client.GetFromJsonAsync<ThesisDTO[]>("api/Thesis");

        Assert.NotNull(theses);
        Assert.False(theses.Length < 3);
    }

    [Fact]
    public async Task Get_Returns_correct_Thesis_from_name()
    {
        var theses = await _client.GetFromJsonAsync<MinimalThesisDTO[]>("api/Thesis");

        Assert.Equal("A study on why notepad is the best IDE", theses[theses.Length - 1].Name);
        Assert.DoesNotContain(theses, t => t.Name == ".NET framework");
    }

    [Fact]
    public async Task Get_Returns_correct_Teacher_from_Thesis()
    {
        var theses = await _client.GetFromJsonAsync<MinimalThesisDTO[]>("api/Thesis");

        // Assert.Contains(theses, t => t.Teacher.Name == "Thore");
        Assert.Equal("Thore", theses[theses.Length - 1].TeacherName);
        Assert.DoesNotContain(theses, t => t.Name == ".NET framework");
    }

    [Fact]
    public async Task Get_Returns_description()
    {
        var theses = await _client.GetFromJsonAsync<MinimalThesisDTO[]>("api/Thesis");

        Assert.NotNull(theses[0].Description);
    }
} */