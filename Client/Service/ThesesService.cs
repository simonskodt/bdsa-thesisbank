using System.Net.Http.Json;

namespace Client.Service;

public class ThesesService : IThesisService
{

    private readonly HttpClient _http;

    public ThesesService(HttpClient http) {
        _http = http;
    }
    
    public async Task<List<Thesis>> GetAllTheses()
    {
        return await _http.GetFromJsonAsync<List<Thesis>>("api/Theses");
    }

    public Task<Thesis> GetThesis(int id)
    {
        throw new NotImplementedException();
    }
}