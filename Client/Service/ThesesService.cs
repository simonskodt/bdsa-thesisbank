using System.Net.Http.Json;

namespace ThesisBank.Client.Service;

public class ThesesService : IThesesService
{
    private readonly HttpClient _http;

    public BlogService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<List<Thesis>> GetThesesPosts()
    {
        return await _http.GetFromJsonAsync<List<Thesis>>("api/Thesis");
    }

    public Task<Thesis> GetThesis(int id)
    {
        throw new NotImplementedException();
    }
}