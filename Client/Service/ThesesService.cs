using System.Net.Http.Json;

namespace ThesisBank.Client.Service;

public class ThesesService : IThesesService
{

    private readonly HttpClient _http;

    public ThesesService(HttpClient http) {
        _http = http;
    }
    
    public async Task<List<Thesis>> GetAllTheses()
    {
        return await _http.GetFromJsonAsync<List<Thesis>>("api/controller");
    }

    public Task<Thesis> GetThesis(int id)
    {
        throw new NotImplementedException();
    }
}