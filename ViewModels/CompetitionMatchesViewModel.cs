using LiveScoresApp.Models;
using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class CompetitionMatchesViewModel
    {
        public string CompetitionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmblemUrl { get; set; } = string.Empty;
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
