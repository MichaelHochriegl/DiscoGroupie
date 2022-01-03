using System.Reflection;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Services;

public class CommandHandlerService : DiscordClientService
{
    private readonly ILogger<CommandHandlerService> _logger;
    private readonly IServiceProvider _provider;
    private readonly IConfiguration _configuration;
    private readonly CommandService _commandService;

    public CommandHandlerService(DiscordSocketClient client,
        ILogger<CommandHandlerService> logger,
        IServiceProvider provider,
        IConfiguration configuration,
        CommandService commandService) : base(client, logger)
    {
        _logger = logger;
        _provider = provider;
        _configuration = configuration;
        _commandService = commandService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Client.MessageReceived += HandleMessage;
        _commandService.CommandExecuted += CommandExecutedAsync;
        await _commandService.AddModulesAsync(Assembly.GetAssembly(typeof(SetupHost)), _provider);
    }
    
    private async Task HandleMessage(SocketMessage incomingMessage)
    {
        if (incomingMessage is not SocketUserMessage { Source: MessageSource.User } message) return;

        var argPos = 0;
        var prefix = _configuration.GetValue<string>("DiscordBotSettings:Prefix");

        if (!message.HasStringPrefix(prefix, ref argPos) 
            && !message.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

        var context = new SocketCommandContext(Client, message);
        _logger.LogDebug("message: '{Message}'", message.Content);

        await _commandService.ExecuteAsync(context, argPos, _provider);
    }
        
    private async Task CommandExecutedAsync(Optional<CommandInfo> command,
        ICommandContext context,
        IResult result)
    {
        if (!command.IsSpecified)
        {
            Logger.LogWarning("Command '{Command}' unknown", command);
            return;
        }
                
            
        Logger.LogInformation("User {User} attempted to use command {Command}",
            context.User, command.Value.Name);

        if (!command.IsSpecified || result.IsSuccess)
            return;

        await context.Channel.SendMessageAsync($"Error: {result}");
    }
}