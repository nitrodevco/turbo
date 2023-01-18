using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using Turbo.Core.Configuration;
using Turbo.Database.Context;
using Turbo.Main.Configuration;
using Turbo.Main.Extensions;

namespace Turbo.Main
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }

            catch (Exception error)
            {
                Log.Error(error.Message);
            }
        }

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

                    var connectionString = $"server={turboConfig.DatabaseHost};user={turboConfig.DatabaseUser};password={turboConfig.DatabasePassword};database={turboConfig.DatabaseName}";

                    services.AddDbContext<IEmulatorContext, TurboContext>(options => options
                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                        {
                            options.MigrationsAssembly("Turbo.Main");
                        })
                        .EnableSensitiveDataLogging(turboConfig.DatabaseLoggingEnabled)
                        .EnableDetailedErrors()
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    );

                    services.AddRepositories();
                    services.AddManagers();
                    services.AddFactories();
                    services.AddNetworking();

                    // Emulator
                    services.AddHostedService<TurboEmulator>();
                }).UseSerilog();
    }
}
