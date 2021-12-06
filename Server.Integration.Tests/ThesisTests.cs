using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Server.Integration.Tests;

public class ThesisTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ThesisTests(CustomWebApplicationFactory factory){
        _factory = factory;
    }

    [Fact]
    public void Get_Returns_Theses()
    {
        
    }
}