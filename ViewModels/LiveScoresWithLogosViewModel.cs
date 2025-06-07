using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class LiveScoresWithLogosViewModel
    {
        public List<CompetitionMatchesViewModel> Competitions { get; set; }
            = new List<CompetitionMatchesViewModel>();
    }
}
