using Discord.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Modules;

public class BotManagementModule : ModuleBase<SocketCommandContext>
{
    private readonly ILogger<BotManagementModule> _logger;
    private readonly IHost _host;

    public BotManagementModule(ILogger<BotManagementModule> logger, IHost host)
    {
        _logger = logger;
        _host = host;
    }
    
    [Command("shutdown")]
    [RequireOwner]
    public async Task Shutdown()
    {
        _logger.LogInformation("User {Username} used the {Command} command!",
            Context.User.Username,
            nameof(Shutdown));
        await _host.StopAsync();
    }
}