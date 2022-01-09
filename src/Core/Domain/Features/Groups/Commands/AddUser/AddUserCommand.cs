using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Core.Domain.Features.Groups.Commands.AddUser;

public class AddUserCommand : IRequest<Result<SocketGuildUser>>
{
    public SocketCommandContext Context { get; set; }
    public SocketGuildUser UserToAdd { get; set; }
}

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<SocketGuildUser>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public AddUserCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SocketGuildUser>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var group = await _dbContext.Groups
            .FirstOrDefaultAsync(x => x.GroupOwner == request.Context.User.Id, cancellationToken);
        
        if (group is null) return Result<SocketGuildUser>
            .Error("You are not a group owner, only group owners can execute this command");

        var isUserToAddInGuild = request.Context.Guild.Users.Any(x => x.Id == request.UserToAdd.Id);
        
        if (isUserToAddInGuild is false) return Result<SocketGuildUser>
            .Error("The mentioned user is not present in this guild");

        var groupRole = request.Context.Guild.Roles.FirstOrDefault(x => x.Id == group.GroupId);
        
        if (groupRole is null) return Result<SocketGuildUser>.Error("Could not retrieve the role for this group!");

        await request.UserToAdd.AddRoleAsync(groupRole);
        
        return Result<SocketGuildUser>.Success(request.UserToAdd);
    }
}
