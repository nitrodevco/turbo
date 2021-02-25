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

namespace Turbo.Main
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        public static async Task Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            BuildConfig(configBuilder);
            IConfiguration config = configBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            await CreateHostBuilder(args, config).Start();
        }

        /// <summary>
        /// This method sets the hosts configuration. Good to know:
        /// - Appsettings.json is considered being the base config file
        /// - Either appsettings.Development.json or appsettings.Production.json can be used to override values in appsettings.json for different environments
        /// - Running this project in Visual Studio will automatically use the appsettings.Development.json
        /// </summary>
        /// <param name="builder"></param>
        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        private static IEmulator CreateHostBuilder(string[] args, IConfiguration config)
        {
            var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                context.Configuration = config;

                // Configuration
                var turboConfig = new TurboConfig();
                context.Configuration.Bind(TurboConfig.Turbo, turboConfig);
                services.AddSingleton<IEmulatorConfig>(turboConfig);

                // DB Context
                services.AddDbContext<IEmulatorContext, TurboContext>(options => options
                    .UseMySql(
                        $"server={turboConfig.DatabaseHost};user={turboConfig.DatabaseUser};password={turboConfig.DatabasePassword};database={turboConfig.DatabaseName}",
                        new MySqlServerVersion(new Version(turboConfig.MySqlVersion))
                    )
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                );

                // Singletons
                services.AddSingleton<IEmulator, TurboEmulator>();
                services.AddSingleton<IPluginManager, TurboPluginManager>();
                services.AddSingleton<INetworkEventLoopGroup, NetworkEventLoopGroup>();
                services.AddSingleton<IGameServer, GameServer>();
                services.AddSingleton<IWSGameServer, WSGameServer>();
                services.AddSingleton<IRestServer, RestServer>();
            })
            .UseSerilog()
            .Build();

            return ActivatorUtilities.CreateInstance<TurboEmulator>(host.Services);
        }
    }
}
