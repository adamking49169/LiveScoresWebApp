# LiveScoresApp

LiveScoresApp is an ASP.NET Core MVC application that displays football data using the [Football-Data.org](https://www.football-data.org/) API. It shows live scores, recent results, league tables and top scorers for the competitions configured in the app settings.

## Features

- Live match scores with minute updates
- Recent results with the ability to expand older matches
- League standings for each competition
- Top scorer tables
- Basic caching and rate limiting to reduce API calls

## Requirements

- [.NET SDK 9.0](https://dotnet.microsoft.com/) or later
- A free API key from [Football-Data.org](https://www.football-data.org/)

## Setup

1. Clone this repository.
2. Obtain an API key from Football-Data.org.
3. Update `appsettings.json` with your key and the competitions you want to follow:

```json
"FootballData": {
  "ApiKey": "YOUR_KEY_HERE",
  "BaseUrl": "https://api.football-data.org/v4/",
  "CompetitionIds": [ "PL", "PD", "BL1" ]
}
```

4. Restore packages and run the application:

```bash
# from the repository root
 dotnet run --project LiveScoresApp.csproj
```

The site defaults to the `Scores/Live` page and will be available on `http://localhost:5000` (or the port chosen by ASP.NET).

## Customising Competitions

Edit the `CompetitionIds` array in `appsettings.json` to include any competitions supported by the Football-Data API. The examples include Premier League (`PL`), La Liga (`PD`), Bundesliga (`BL1`), Champions League (`CL`), Ligue 1 (`FL1`), Serie A (`SA`) and Eredivisie (`DED`).

## Project Structure

- **Controllers** – MVC controllers for the home page and scores
- **Models** – Data transfer models that map to the API
- **Services** – Clients and helpers used to call the API
- **ViewModels** – Models passed to the Razor views
- **Views** – Razor pages for live scores, results, standings and scorers
- **wwwroot** – Static assets (CSS, JavaScript)
