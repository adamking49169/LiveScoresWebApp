using System.Text.Json.Serialization;

namespace LiveScoresApp.Models
{
    public class Player
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Scorer
    {
        [JsonPropertyName("player")]
        public Player Player { get; set; } = new Player();

        [JsonPropertyName("team")]
        public Team Team { get; set; } = new Team();

        [JsonPropertyName("goals")]
        public int Goals { get; set; }
    }

    public class ScorersResponse
    {
        [JsonPropertyName("competition")]
        public Competition Competition { get; set; } = new Competition();

        [JsonPropertyName("scorers")]
        public List<Scorer> Scorers { get; set; } = new List<Scorer>();
    }
}