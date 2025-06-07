using LiveScoresApp.Models;
using LiveScoresApp.Services;
using LiveScoresApp.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Linq;

public class FootballDataClient
{
    private readonly HttpClient _client;
    private readonly FootballDataOptions _opts;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

    public FootballDataClient(
        HttpClient client,
        IOptions<FootballDataOptions> opts,
        IMemoryCache cache)
    {
        _client = client;
        _opts = opts.Value;
        _cache = cache;
    }

    private async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
    {
        if (_cache.TryGetValue(key, out T cached))
        {
            return cached;
        }

        T result = await factory();
        _cache.Set(key, result, _cacheDuration);
        return result;
    }

    public async Task<List<CompetitionMatchesViewModel>> GetRecentResultsWithLogosAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();

        var tasks = compIds.Select(compId =>
            GetOrCreateAsync(
                $"recent_{compId}",
                async () =>
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
                    catch (HttpRequestException ex)
                    {
                        Console.Error.WriteLine($"Error fetching live matches: {ex.Message}");
                        return new CompetitionMatchesViewModel
                        {
                            CompetitionId = compId,
                            Name = compId,
                            EmblemUrl = string.Empty,
                            Matches = new List<Match>()
                        };
                    }
                }));
        return (await Task.WhenAll(tasks)).ToList();
    }


    public async Task<List<CompetitionMatchesViewModel>> GetAllLiveMatchesWithLogosAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();
        var tasks = compIds.Select(compId =>
        GetOrCreateAsync(
            $"live_{compId}",
            async () =>
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
                catch (HttpRequestException ex)
                {
                    Console.Error.WriteLine($"Error fetching live matches: {ex.Message}");
                    return new CompetitionMatchesViewModel
                    {
                        CompetitionId = compId,
                        Name = compId,
                        EmblemUrl = string.Empty,
                        Matches = new List<Match>()
                    };
                }
            }));

        return (await Task.WhenAll(tasks)).ToList();
    }


    public async Task<Dictionary<string, List<Match>>> GetAllLiveMatchesAsync()
    {
        var ids = _opts.CompetitionIds.Distinct();

        var tasks = _opts.CompetitionIds
                  .Select(compId =>
                GetOrCreateAsync(
                    $"liveonly_{compId}",
                    async () =>
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
                    }));
        var results = await Task.WhenAll(tasks);
        return results.ToDictionary(r => r.compId, r => r.matches);
    }

    public async Task<List<CompetitionScorersViewModel>> GetTopScorersWithLogosAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();

        var tasks = compIds.Select(compId =>
            GetOrCreateAsync(
                $"scorers_{compId}",
                async () =>
                {
                    try
                    {
                        var resp = await _client.GetFromJsonAsync<ScorersResponse>(
                            $"competitions/{compId}/scorers?limit=10");

                        return new CompetitionScorersViewModel
                        {
                            CompetitionId = compId,
                            Name = resp!.Competition.Name,
                            EmblemUrl = resp.Competition.Emblem,
                            Scorers = resp.Scorers
                        };
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.Error.WriteLine($"Error fetching scorers: {ex.Message}");
                        return new CompetitionScorersViewModel
                        {
                            CompetitionId = compId,
                            Name = compId,
                            EmblemUrl = string.Empty,
                            Scorers = new List<Scorer>()
                        };
                    }
                }));

        return (await Task.WhenAll(tasks)).ToList();
    }

    public async Task<List<CompetitionStandingsViewModel>> GetStandingsAsync()
    {
        var compIds = _opts.CompetitionIds
            .Select(id => id.Trim().ToUpperInvariant())
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct();

        var tasks = compIds.Select(compId =>
            GetOrCreateAsync(
                $"standings_{compId}",
                async () =>
                {
                    try
                    {
                        var resp = await _client
                            .GetFromJsonAsync<StandingsResponse>(
                                $"competitions/{compId}/standings");

                        var table = resp!.Standings
                            .FirstOrDefault(s => s.Type == "TOTAL")?.Table
                            ?? new List<TableRow>();

                        return new CompetitionStandingsViewModel
                        {
                            CompetitionId = compId,
                            Name = resp.Competition.Name,
                            EmblemUrl = resp.Competition.Emblem,
                            Table = table
                        };
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.Error.WriteLine($"Error fetching standings: {ex.Message}");
                        return new CompetitionStandingsViewModel
                        {
                            CompetitionId = compId,
                            Name = compId,
                            EmblemUrl = string.Empty,
                            Table = new List<TableRow>()
                        };
                    }
                }));

        return (await Task.WhenAll(tasks)).ToList();
    }
}
