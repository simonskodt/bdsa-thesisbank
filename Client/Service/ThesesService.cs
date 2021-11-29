using System.Net.Http.Json;

namespace ThesisBank.Client.Service;

public class ThesesService : IThesesService
{

    // private readonly HttpClient _http;

    // public ThesesService(HttpClient http) {
    //     _http = http;
    // }

    // public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    // public async Task<List<Thesis>> GetAllTheses()
    public Task<Thesis[]> GetAllTheses()
    {
        // return await _http.GetFromJsonAsync<List<Thesis>>("api/controller");
        var teacher = new Teacher{Id = 2, Name = "Rasmus", Email = "Rasmus@itu.dk"};

        return Task.FromResult(Enumerable.Range(1,5).Select(thesis => new Thesis {
            Id = 1, 
            Name = "Linq", 
            Teacher = teacher
        })).ToArray();

        // return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = startDate.AddDays(index),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // }).ToArray());
        
    }

    public Task<Thesis> GetThesis(int id)
    {
        throw new NotImplementedException();
    }
}