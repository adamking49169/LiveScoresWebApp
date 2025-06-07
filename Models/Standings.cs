using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LiveScoresApp.Models
{
    public class TableRow
    {
        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("team")]
        public Team Team { get; set; } = new Team();

        [JsonPropertyName("playedGames")]
        public int PlayedGames { get; set; }

        [JsonPropertyName("won")]
        public int Won { get; set; }

        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("lost")]
        public int Lost { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }
    }

    public class Standing
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("table")]
        public List<TableRow> Table { get; set; } = new List<TableRow>();
    }

    public class StandingsResponse
    {
        [JsonPropertyName("competition")]
        public Competition Competition { get; set; } = new Competition();

        [JsonPropertyName("standings")]
        public List<Standing> Standings { get; set; } = new List<Standing>();
    }
}