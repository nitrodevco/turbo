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
using Turbo.Database.Repositories;

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
                        .EnableDetailedErrors()
                    );

                    // Repositories
                    services.AddScoped<IHabboRepository, HabboRepository>();

                    // Singletons
                    services.AddSingleton<IPluginManager, TurboPluginManager>();
                    services.AddSingleton<INetworkEventLoopGroup, NetworkEventLoopGroup>();
                    services.AddSingleton<IGameServer, GameServer>();
                    services.AddSingleton<IWSGameServer, WSGameServer>();
                    services.AddSingleton<IRestServer, RestServer>();

                    // Emulator
                    services.AddHostedService<TurboEmulator>();
                }).UseSerilog();
    }
}
