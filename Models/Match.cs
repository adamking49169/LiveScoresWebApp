using LiveScoresApp.Models;
using System.Text.Json.Serialization;

namespace LiveScoresApp.Models
{
    public class Score
    {
        [JsonPropertyName("fullTime")]
        public ScorePeriod FullTime { get; set; } = new ScorePeriod();

        [JsonPropertyName("halfTime")]
        public ScorePeriod HalfTime { get; set; } = new ScorePeriod();
    }

    public class Team
    {
        public string Name { get; set; }
    }

    public class Match
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("utcDate")]
        public DateTime UtcDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("minute")]
        public int? Minute { get; set; }    

        [JsonPropertyName("homeTeam")]
        public Team HomeTeam { get; set; } = default!;

        [JsonPropertyName("awayTeam")]
        public Team AwayTeam { get; set; } = default!;

        [JsonPropertyName("score")]
        public Score Score { get; set; } = new Score();
    }

    public class MatchesResponse
    {
        [JsonPropertyName("competition")]
        public Competition Competition { get; set; } = new Competition();

        [JsonPropertyName("matches")]
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}