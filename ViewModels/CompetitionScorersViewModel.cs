using LiveScoresApp.Models;
using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class CompetitionScorersViewModel
    {
        public string CompetitionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmblemUrl { get; set; } = string.Empty;
        public List<Scorer> Scorers { get; set; } = new List<Scorer>();
    }
}