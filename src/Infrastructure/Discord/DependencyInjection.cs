using DiscoGroupie.Core.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoGroupie.Infrastructure.Discord;

public static class DependencyInjection
{
    public static IServiceCollection AddDiscord(this IServiceCollection services)
    {
        services.AddTransient<IDiscoGroupieModuleBase>();
        return services;
    }
}