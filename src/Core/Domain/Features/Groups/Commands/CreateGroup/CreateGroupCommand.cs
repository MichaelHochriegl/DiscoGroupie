using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using Discord;
using Discord.Commands;
using Discord.Rest;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Core.Domain.Features.Groups.Commands.CreateGroup;

public class CreateGroupCommand : IRequest<Result<Group>>
{
    public ulong GuildId { get; set; }
    public ulong OwnerId { get; set; }
    public string GroupName { get; set; }
    public SocketCommandContext Context { get; set; }
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Result<Group>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public CreateGroupCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Group>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var guildSettings = await _dbContext.Guilds.FirstOrDefaultAsync(x => x.GuildId == request.GuildId);
        
        if (guildSettings is null) return Result<Group>.Error("Guild not yet set up for Group creation");

        var groupName = $"{guildSettings.GroupNamePrefix}{request.GroupName}";
        var isGroupPresent =
            await _dbContext.Groups.AnyAsync(x => x.GuildId == request.GuildId 
                                                  && x.GroupName == groupName,
                cancellationToken);
        if (isGroupPresent) return Result<Group>
            .Error($"A Group with the name '{request.GroupName}' is already present on this server");
        
        var isTextChannelPresent = request.Context.Guild.Channels.Any(x => x.Name.Contains(groupName));
        var isVoiceChannelPresent = request.Context.Guild.VoiceChannels.Any(x => x.Name.Contains(groupName));
        
        if (isTextChannelPresent || isVoiceChannelPresent) return Result<Group>
            .Error("Channel/s with this name is already present on this server");

        var createdTextChannel = await CreateTextChannel(groupName, guildSettings, request.Context);
        var createdVoiceChannel = await CreateVoiceChannel(groupName, guildSettings, request.Context);
        var createdGroupRole = await CreateGroupRole(groupName, request.Context);

        var guildUser = request.Context.User as IGuildUser;
        if (guildUser is null) return Result<Group>.Error("There was an unexpected error with the user"); 
        await AssignPermissions(guildUser, createdGroupRole, createdTextChannel, createdVoiceChannel, request.Context);

        var group = new Group(createdGroupRole.Id, request.GuildId, request.OwnerId, request.GroupName);
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Group>.Success(group);
    }

    private async Task AssignPermissions(IGuildUser guildUser, RestRole createdGroupRole,
        RestTextChannel createdTextChannel, RestVoiceChannel createdVoiceChannel, SocketCommandContext context)
    {
        await guildUser.AddRoleAsync(createdGroupRole);

        var groupRolePermissions = new OverwritePermissions(
            viewChannel: PermValue.Allow,
            sendMessages: PermValue.Allow,
            sendMessagesInThreads: PermValue.Allow,
            speak: PermValue.Allow,
            useVoiceActivation: PermValue.Allow,
            readMessageHistory: PermValue.Allow,
            addReactions: PermValue.Allow,
            createPublicThreads: PermValue.Allow,
            embedLinks: PermValue.Allow,
            connect: PermValue.Allow);

        await createdTextChannel.AddPermissionOverwriteAsync(context.Guild.EveryoneRole,
            OverwritePermissions.DenyAll(createdTextChannel));
        await createdTextChannel.AddPermissionOverwriteAsync(createdGroupRole,
            groupRolePermissions);
        await createdTextChannel.AddPermissionOverwriteAsync(context.User,
            OverwritePermissions.AllowAll(createdTextChannel));

        await createdVoiceChannel.AddPermissionOverwriteAsync(context.Guild.EveryoneRole,
            OverwritePermissions.DenyAll(createdVoiceChannel));
        await createdVoiceChannel.AddPermissionOverwriteAsync(createdGroupRole,
            groupRolePermissions);
        await createdVoiceChannel.AddPermissionOverwriteAsync(context.User,
            OverwritePermissions.AllowAll(createdVoiceChannel));
    }

    private async Task<RestRole> CreateGroupRole(string groupName, SocketCommandContext context)
    {
        var createdGroupRole = await context.Guild.CreateRoleAsync(groupName,
            new GuildPermissions(),
            Color.LightOrange,
            false,
            new RequestOptions());
        return createdGroupRole;
    }

    private async Task<RestVoiceChannel> CreateVoiceChannel(string groupName, Guild guildSettings,
        SocketCommandContext context)
    {
        var createdVoiceChannel = await context.Guild.CreateVoiceChannelAsync(groupName,
            config =>
            {
                if (guildSettings.GroupVoiceSectionId == default) return;
                config.CategoryId = guildSettings.GroupVoiceSectionId;
            });
        return createdVoiceChannel;
    }

    private async Task<RestTextChannel> CreateTextChannel(string groupName, Guild guildSettings,
        SocketCommandContext context)
    {
        var createdTextChannel = await context.Guild.CreateTextChannelAsync(groupName,
            config =>
            {
                if (guildSettings.GroupTextSectionId == default) return;
                config.CategoryId = guildSettings.GroupTextSectionId;
            });
        return createdTextChannel;
    }
}
