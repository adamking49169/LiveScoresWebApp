using LiveScoresApp.Models;
using System.Collections.Generic;

namespace LiveScoresApp.ViewModels
{
    public class CompetitionStandingsViewModel
    {
        public string CompetitionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EmblemUrl { get; set; } = string.Empty;
        public List<TableRow> Table { get; set; } = new List<TableRow>();
    }
}