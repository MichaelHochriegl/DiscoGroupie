using DiscoGroupie.Infrastructure.Discord;
using DiscoGroupie.UI.Bot;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("PzDiscoB starting up...");
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices(services =>
        {
            // services.AddHostedService<Worker>();
            // services.AddPersistence(configuration);
        })
        .SetupDiscord()
        .Build();
    
    // using (var scope = host.Services.CreateScope())
    // {
    //     var db = scope.ServiceProvider.GetRequiredService<DiscoGroupieNpsqlDbContext>();
    //     db.Database.Migrate();
    // }

    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "PzDiscoB failed to start!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}