using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Core.Domain.Features.Groups.Commands.RemoveUser;

public class RemoveUserCommand : IRequest<Result<SocketGuildUser>>
{
    public SocketCommandContext Context { get; set; }
    public SocketGuildUser UserToRemove { get; set; }
}

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Result<SocketGuildUser>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public RemoveUserCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SocketGuildUser>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(x => x.GroupOwner == request.Context.User.Id, cancellationToken);
        
        if (group is null) return Result<SocketGuildUser>
            .Error("You are not a group owner, only group owners can execute this command");

        var isUserToAddInGuild = request.Context.Guild.Users.Any(x => x.Id == request.UserToRemove.Id);
        
        if (isUserToAddInGuild is false) return Result<SocketGuildUser>
            .Error("The mentioned user is not present in this guild");

        var groupRole = request.Context.Guild.Roles.FirstOrDefault(x => x.Id == group.GroupId);
        
        if (groupRole is null) return Result<SocketGuildUser>.Error("Could not retrieve the role for this group!");

        await request.UserToRemove.RemoveRoleAsync(groupRole);
        
        return Result<SocketGuildUser>.Success(request.UserToRemove);
    }
}
