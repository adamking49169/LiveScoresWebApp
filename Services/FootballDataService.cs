using System.Net.Http.Json;
using LiveScoresApp.Models;

namespace LiveScoresApp.Services
{
    public class FootballDataService
    {
        private readonly HttpClient _client;
        public FootballDataService(HttpClient client) => _client = client;

        public async Task<List<Match>> GetLiveMatchesAsync()
        {
            try
            {
                // Use the numeric competition ID '2021' for Premier League in v4
                var response = await _client.GetFromJsonAsync<MatchesResponse>(
                    "competitions/2021/matches?status=LIVE");

                return response?.Matches ?? new List<Match>();
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the error as needed
                // For now, return an empty list so the UI can show a friendly message
                Console.Error.WriteLine($"Error fetching live matches: {ex.Message}");
                return new List<Match>();
            }
        }
    }
}