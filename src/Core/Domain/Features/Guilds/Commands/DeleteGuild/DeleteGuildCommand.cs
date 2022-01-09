using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using MediatR;

namespace DiscoGroupie.Core.Domain.Features.Guilds.Commands.DeleteGuild;

public class DeleteGuildCommand : IRequest<Result<Guild>>
{
    public ulong GuildId { get; set; }
}

public class DeleteGuildCommandHandler : IRequestHandler<DeleteGuildCommand, Result<Guild>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public DeleteGuildCommandHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guild>> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
    {
        var guild = await _dbContext.Guilds.FindAsync(request.GuildId);
        
        if (guild is null) return Result<Guild>.NotFound();

        _dbContext.Guilds.Remove(guild);
        
        return Result<Guild>.Success(guild);
    }
}
