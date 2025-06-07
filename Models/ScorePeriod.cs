using System.Text.Json.Serialization;

namespace LiveScoresApp.Models
{
    public class ScorePeriod
    {
        [JsonPropertyName("home")]
        public int? Home { get; set; }

        [JsonPropertyName("away")]
        public int? Away { get; set; }
    }
}
