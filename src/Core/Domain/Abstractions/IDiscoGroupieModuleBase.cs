using Discord.Commands;

namespace DiscoGroupie.Core.Domain.Abstractions;

public interface IDiscoGroupieModuleBase : IModuleBase
{
    public SocketCommandContext Context { get; set; }
    
}