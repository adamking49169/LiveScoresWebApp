using LiveScoresApp.Models;
using LiveScoresApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class ScoresController : Controller
{
    private readonly FootballDataClient _client;
    public ScoresController(FootballDataClient client) => _client = client;

    public async Task<IActionResult> Results()
    {
        var comps = await _client.GetRecentResultsWithLogosAsync();
        var vm = new LiveScoresWithLogosViewModel { Competitions = comps };
        return View(vm);
    }
    public IActionResult Index() => RedirectToAction(nameof(Live));

    public async Task<IActionResult> Live()
    {
        var comps = await _client.GetAllLiveMatchesWithLogosAsync();
        var vm = new LiveScoresWithLogosViewModel { Competitions = comps };
        return View(vm);
    }

}

public class LiveScoresViewModel
{
    public Dictionary<string, List<Match>> MatchesByCompetition { get; set; } = new();
}
