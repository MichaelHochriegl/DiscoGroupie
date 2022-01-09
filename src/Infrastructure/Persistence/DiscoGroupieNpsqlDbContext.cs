using DiscoGroupie.Core.Domain.Abstractions;
using DiscoGroupie.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Infrastructure.Persistence;

public class DiscoGroupieNpsqlDbContext : DbContext, IDiscoGroupDbContext
{
    public DbSet<Guild> Guilds { get; set; }
    public DbSet<Group> Groups { get; set; }

    public DiscoGroupieNpsqlDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}