using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class TopScorersViewModel
    {
        public List<CompetitionScorersViewModel> Competitions { get; set; }
            = new List<CompetitionScorersViewModel>();
    }
}