using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Services;

public class BotStatusService : DiscordClientService
{
    private readonly ILogger<BotStatusService> _logger;

    public BotStatusService(DiscordSocketClient client, ILogger<BotStatusService> logger) : base(client, logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Client.WaitForReadyAsync(stoppingToken);
        _logger.LogInformation("DiscordBot: Client is ready!");

        await Client.SetActivityAsync(new Game("Waiting at the reception desk"));
    }
}