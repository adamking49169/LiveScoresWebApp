using System.Text.Json.Serialization;

namespace LiveScoresApp.Models
{
    public class Competition
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("emblem")]
        public string Emblem { get; set; } = string.Empty;
    }
}
