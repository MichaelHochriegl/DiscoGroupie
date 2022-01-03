using DiscoGroupie.Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoGroupie.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DiscoGroupieNpsqlDbContext>(options =>
        {
            var connectionStringSelector = configuration
                .GetValue<string>("ApplicationSettings:UsedConnectionString");
            var connectionString = configuration.GetConnectionString(connectionStringSelector);
            options.UseNpgsql(connectionString, opt => 
                opt.MigrationsAssembly(typeof(DiscoGroupieNpsqlDbContext).Assembly.FullName));
        });
        services.AddTransient<IDiscoGroupDbContext, DiscoGroupieNpsqlDbContext>();

        return services;
    }
}