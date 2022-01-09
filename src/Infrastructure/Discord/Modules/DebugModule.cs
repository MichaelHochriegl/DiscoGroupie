using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Modules;

public class DebugModule : ModuleBase<SocketCommandContext>
{
    private readonly ILogger<DebugModule> _logger;

    public DebugModule(ILogger<DebugModule> logger)
    {
        _logger = logger;
    }
    
    [Command("ping")]
    [Alias("p")]
    public async Task PingAsync()
    {
        _logger.LogInformation("User {Username} used the {Command} command!",
            Context.User.Username,
            nameof(PingAsync));
        await ReplyAsync("pong!");
    }
}