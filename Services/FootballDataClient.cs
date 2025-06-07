using LiveScoresApp.Models;
using LiveScoresApp.Services;
using LiveScoresApp.ViewModels;
using Microsoft.Extensions.Options;

public class FootballDataClient
{
    private readonly HttpClient _client;
    private readonly FootballDataOptions _opts;

    public FootballDataClient(HttpClient client, IOptions<FootballDataOptions> opts)
    {
        _client = client;
        _opts = opts.Value;
    }

    public async Task<List<CompetitionMatchesViewModel>> GetRecentResultsWithLogosAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();

        var tasks = compIds.Select(async compId =>
        {
            try
            {
                var resp = await _client.GetFromJsonAsync<MatchesResponse>(
                    $"competitions/{compId}/matches?status=FINISHED&limit=10");
                return new CompetitionMatchesViewModel
                {
                    CompetitionId = compId,
                    Name = resp!.Competition.Name,
                    EmblemUrl = resp.Competition.Emblem,
                    Matches = resp.Matches
                };
            }
            catch (HttpRequestException)
            {
                return new CompetitionMatchesViewModel
                {
                    CompetitionId = compId,
                    Name = compId,
                    EmblemUrl = string.Empty,
                    Matches = new List<Match>()
                };
            }
        });

        return (await Task.WhenAll(tasks)).ToList();
    }


    public async Task<List<CompetitionMatchesViewModel>> GetAllLiveMatchesWithLogosAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();

        var tasks = compIds.Select(async compId =>
        {
            try
            {
                var resp = await _client
                 .GetFromJsonAsync<MatchesResponse>(
                    $"competitions/{compId}/matches?status=LIVE");

                return new CompetitionMatchesViewModel
                {
                    CompetitionId = compId,
                    Name = resp!.Competition.Name,
                    EmblemUrl = resp.Competition.Emblem,
                    Matches = resp.Matches
                };
            }
            catch (HttpRequestException)
            {
                return new CompetitionMatchesViewModel
                {
                    CompetitionId = compId,
                    Name = compId,
                    EmblemUrl = string.Empty,
                    Matches = new List<Match>()
                };
            }
        });

        return (await Task.WhenAll(tasks)).ToList();
    }


    public async Task<Dictionary<string, List<Match>>> GetAllLiveMatchesAsync()
    {
        var ids = _opts.CompetitionIds.Distinct();

        var tasks = _opts.CompetitionIds
            .Select(async compId =>
            {
                try
                {
                    var resp = await _client.GetFromJsonAsync<MatchesResponse>(
                        $"competitions/{compId}/matches?status=LIVE");
                    return (compId, matches: resp?.Matches ?? new List<Match>());
                }
                catch (HttpRequestException)
                {
                    // swallow or log per‐competition failure
                    return (compId, matches: new List<Match>());
                }
            });

        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(r => r.compId, r => r.matches);
    }
}
