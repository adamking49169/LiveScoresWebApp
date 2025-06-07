using System.Collections.Generic;

public class FootballDataOptions
{
    public string ApiKey { get; set; } = default!;
    public string BaseUrl { get; set; } = default!;
    public List<string> CompetitionIds { get; set; } = new();
}
