using LiveScoresApp.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration
//builder.Services.Configure<FootballDataOptions>(
//    builder.Configuration.GetSection("FootballData")
//);

builder.Services
    .Configure<FootballDataOptions>(builder.Configuration.GetSection("FootballData"))
    .AddHttpClient<FootballDataClient>((sp, client) => {
        var opts = sp.GetRequiredService<IOptions<FootballDataOptions>>().Value;
        client.BaseAddress = new Uri(opts.BaseUrl);
        client.DefaultRequestHeaders.Add("X-Auth-Token", opts.ApiKey);
    });

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Scores}/{action=Live}/{id?}");

app.Run();