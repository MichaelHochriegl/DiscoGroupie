using DiscoGroupie.Infrastructure.Discord.Services;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscoGroupie.Infrastructure.Discord;

public static class SetupHost
{
    public static IHostBuilder SetupDiscord(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureDiscordHost((context, config) =>
        {
            config.SocketConfig = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 200,
                GatewayIntents = GatewayIntents.All
            };

            config.Token = context.Configuration.GetValue<string>("DiscordBotSettings:Token");
        });

        hostBuilder.UseCommandService((context, config) =>
        {
            config.CaseSensitiveCommands = false;
            config.LogLevel = LogSeverity.Verbose;
            config.DefaultRunMode = RunMode.Async;
        });

        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<CommandHandlerService>();
            services.AddHostedService<BotStatusService>();
                
        });

        hostBuilder.UseConsoleLifetime();
            
        return hostBuilder;
    }
}