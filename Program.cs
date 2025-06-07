using LiveScoresApp.Services;
using Microsoft.Extensions.Options;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
//builder.Services.Configure<FootballDataOptions>(
//    builder.Configuration.GetSection("FootballData")
//);

builder.Services
    .Configure<FootballDataOptions>(builder.Configuration.GetSection("FootballData"))
     .AddMemoryCache()
   .AddHttpClient<FootballDataClient>((sp, client) =>
   {
       var opts = sp.GetRequiredService<IOptions<FootballDataOptions>>().Value;
        client.BaseAddress = new Uri(opts.BaseUrl);
        client.DefaultRequestHeaders.Add("X-Auth-Token", opts.ApiKey);
    })
   .AddHttpMessageHandler(() =>
        new RateLimitedHandler(permitLimit: 10, window: TimeSpan.FromSeconds(20)));
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Scores}/{action=Live}/{id?}");

app.Run();