using Ardalis.Result;
using DiscoGroupie.Core.Domain.Features.Guilds.Commands.CreateGuild;
using Discord;
using Discord.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Modules;

public class BotManagementModule : ModuleBase<SocketCommandContext>
{
    private readonly ILogger<BotManagementModule> _logger;
    private readonly IHost _host;
    private readonly IServiceProvider _service;

    public BotManagementModule(ILogger<BotManagementModule> logger,
        IHost host,
        IServiceProvider service)
    {
        _logger = logger;
        _host = host;
        _service = service;
    }

    [Command("setup")]
    [Name("setup")]
    [Summary("Setup the necessary settings for the DiscoGroupie bot")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task Setup(string prefix = "!dg ",
        string groupNamePrefix = "Group_",
        ulong groupTextSectionId = default,
        ulong groupVoiceSectionId = default)
    {
        using var scope = _service.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new CreateGuildCommand
        {
            GuildId = Context.Guild.Id,
            GroupTextSectionId = groupTextSectionId,
            GroupVoiceSectionId = groupVoiceSectionId,
            Prefix = prefix,
            GroupNamePrefix = groupNamePrefix
        };
        
        var result = await mediator.Send(command);
        await ReplyAsync(embed: result.ToEmbed(nameof(Setup)));
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