using Ardalis.Result;
using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using MediatR;

namespace DiscoGroupie.Core.Domain.Features.Guilds.Queries.GetGuildById;

public class GetGuildByIdQuery : IRequest<Result<Guild>>
{
    public ulong GuildId { get; set; }
}

public class GetGuildByIdQueryHandler : IRequestHandler<GetGuildByIdQuery, Result<Guild>>
{
    private readonly IDiscoGroupDbContext _dbContext;

    public GetGuildByIdQueryHandler(IDiscoGroupDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guild>> Handle(GetGuildByIdQuery request, CancellationToken cancellationToken)
    {
        var guild = await _dbContext.Guilds.FindAsync(request.GuildId, cancellationToken);
        
        return guild is null ? Result<Guild>.NotFound() : Result<Guild>.Success(guild);
    }
}
