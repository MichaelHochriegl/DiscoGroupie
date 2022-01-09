using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoGroupie.Core.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainCore(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetAssembly(typeof(DependencyInjection)));
        
        return services;
    }
}