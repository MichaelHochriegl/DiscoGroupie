using DiscoGroupie.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscoGroupie.Core.Domain.Abstractions;

public interface IDiscoGroupDbContext
{
    public DbSet<Guild> Guilds { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}