using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class StandingsViewModel
    {
        public List<CompetitionStandingsViewModel> Competitions { get; set; }
            = new List<CompetitionStandingsViewModel>();
    }
}