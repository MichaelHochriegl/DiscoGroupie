using DiscoGroupie.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscoGroupie.Infrastructure.Persistence.Configurations;

public class GuildConfiguration : IEntityTypeConfiguration<Guild>
{
    public void Configure(EntityTypeBuilder<Guild> builder)
    {
        builder.HasKey(p => p.GuildId);

        builder.Property(p => p.Prefix)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.GroupNamePrefix)
            .HasMaxLength(100);
    }
}