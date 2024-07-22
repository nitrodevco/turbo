using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Turbo.Core.Configuration;
using Turbo.Database.Context;
using Turbo.Main.Configuration;
using Turbo.Main.Extensions;

namespace Turbo.Main;

[ExcludeFromCodeCoverage]
internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }

        catch (Exception error)
        {
            Console.WriteLine(error);
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();

                // Configuration
                var turboConfig = new TurboConfig();
                hostContext.Configuration.Bind(TurboConfig.Turbo, turboConfig);
                services.AddSingleton<IEmulatorConfig>(turboConfig);

                var connectionString =
                    $"server={turboConfig.DatabaseHost};user={turboConfig.DatabaseUser};password={turboConfig.DatabasePassword};database={turboConfig.DatabaseName}";

                services.AddDbContext<IEmulatorContext, TurboContext>(
                    options =>
                    {
                        options
                            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                                options => { options.MigrationsAssembly("Turbo.Main"); })
                            .ConfigureWarnings(warnings => warnings
                                .Ignore(CoreEventId.RedundantIndexRemoved))
                            .EnableSensitiveDataLogging(turboConfig.DatabaseLoggingEnabled)
                            .EnableDetailedErrors()
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    }
                );

                services.AddEncryption();
                services.AddRepositories();
                services.AddManagers();
                services.AddFactories();
                services.AddNetworking();

                // Emulator
                services.AddHostedService<TurboEmulator>();
            });
    }
}