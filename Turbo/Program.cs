using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Configuration;
using Turbo.Database.Context;
using Turbo.Main.Configuration;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;
using Turbo.Plugins;
using Microsoft.EntityFrameworkCore;
using Turbo.Players;
using Turbo.Rooms;
using Turbo.Database.Repositories;
using Turbo.Database.Repositories.Player;

namespace Turbo.Main
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();

                    // Configuration
                    var turboConfig = new TurboConfig();
                    hostContext.Configuration.Bind(TurboConfig.Turbo, turboConfig);
                    services.AddSingleton<IEmulatorConfig>(turboConfig);

                    // DB Context
                    var connectionString = $"server={turboConfig.DatabaseHost};user={turboConfig.DatabaseUser};password={turboConfig.DatabasePassword};database={turboConfig.DatabaseName}";
                    services.AddDbContext<IEmulatorContext, TurboContext>(options => options
                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors(),
                        ServiceLifetime.Singleton
                    );

                    // Repositories
                    services.AddSingleton<IPlayerRepository, PlayerRepository>();

                    // Singletons
                    services.AddSingleton<IPluginManager, TurboPluginManager>();
                    services.AddSingleton<INetworkEventLoopGroup, NetworkEventLoopGroup>();
                    services.AddSingleton<IGameServer, GameServer>();
                    services.AddSingleton<IWSGameServer, WSGameServer>();
                    services.AddSingleton<IRestServer, RestServer>();

                    services.AddSingleton<IPlayerManager, PlayerManager>();
                    services.AddSingleton<IRoomManager, RoomManager>();

                    // Emulator
                    services.AddHostedService<TurboEmulator>();
                }).UseSerilog();
    }
}
