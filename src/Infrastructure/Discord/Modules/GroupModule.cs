using Ardalis.Result;
using DiscoGroupie.Core.Domain.Features.Groups.Commands.AddUser;
using DiscoGroupie.Core.Domain.Features.Groups.Commands.CreateGroup;
using DiscoGroupie.Core.Domain.Features.Groups.Commands.RemoveUser;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DiscoGroupie.Infrastructure.Discord.Modules;

public class GroupModule : ModuleBase<SocketCommandContext>
{
    private readonly ILogger<GroupModule> _logger;
    private readonly IServiceProvider _service;

    public GroupModule(ILogger<GroupModule> logger, IServiceProvider service)
    {
        _logger = logger;
        _service = service;
    }

    [Command("createGroup")]
    [Name("createGroup")]
    [Summary("Creates a new group for the requesting user")]
    public async Task CreateGroup(string groupName)
    {
        using var scope = _service.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new CreateGroupCommand
        {
            GuildId = Context.Guild.Id,
            GroupName = groupName,
            OwnerId = Context.User.Id,
            Context = Context
        };
        
        var result = await mediator.Send(command);
        await ReplyAsync(embed: result.ToEmbed(nameof(CreateGroup)));
    }

    [Command("addUserToGroup")]
    [Name("addUserToGroup")]
    [Summary("Adds a user to the group of the account executing the command." +
             " The account executing the command must be the group owner")]
    public async Task AddUserToGroup(SocketGuildUser userToAdd)
    {
        using var scope = _service.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new AddUserCommand
        {
            UserToAdd = userToAdd,
            Context = Context
        };

        var result = await mediator.Send(command);
        await ReplyAsync(embed: result.ToEmbed(nameof(AddUserToGroup)));
    }

    [Command("removeUserFromGroup")]
    [Name("removeUserFromGroup")]
    [Summary("Removes the mentioned user from the group of the account executing the command." +
             " The account executing the command must be the group owner")]
    public async Task RemoveUserFromGroup(SocketGuildUser userToRemove)
    {
        using var scope = _service.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var command = new RemoveUserCommand()
        {
            UserToRemove = userToRemove,
            Context = Context
        };

        var result = await mediator.Send(command);
        await ReplyAsync(embed: result.ToEmbed(nameof(RemoveUserFromGroup)));
    }
}