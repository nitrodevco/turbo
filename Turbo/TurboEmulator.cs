using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Networking;
using Turbo.Furniture;
using Turbo.Players;
using Turbo.Plugins;
using Turbo.Rooms;
using Turbo.Security;

namespace Turbo.Main
{
    public class TurboEmulator : IEmulator
    {
        public const int MAJOR = 0;
        public const int MINOR = 0;
        public const int PATCH = 0;

        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<TurboEmulator> _logger;
        private readonly IPluginManager _pluginManager;
        private readonly IServerManager _serverManager;

        private readonly ISecurityManager _securityManager;
        private readonly IFurnitureManager _furnitureManager;
        private readonly IRoomManager _roomManager;
        private readonly IPlayerManager _playerManager;

        public TurboEmulator(
            IHostApplicationLifetime appLifetime,
            ILogger<TurboEmulator> logger,
            IPluginManager pluginManager,
            IServerManager serverManager,
            ISecurityManager securityManager,
            IFurnitureManager furnitureManager,
            IRoomManager roomManager,
            IPlayerManager playerManager)
        {
            _appLifetime = appLifetime;
            _logger = logger;
            _pluginManager = pluginManager;
            _serverManager = serverManager;
            _securityManager = securityManager;
            _furnitureManager = furnitureManager;
            _roomManager = roomManager;
            _playerManager = playerManager;
        }

        /// <summary>
        /// This method is called by the .NET Generic Host.
        /// See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0 for more information.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(@"
                ████████╗██╗   ██╗██████╗ ██████╗  ██████╗ 
                ╚══██╔══╝██║   ██║██╔══██╗██╔══██╗██╔═══██╗
                   ██║   ██║   ██║██████╔╝██████╔╝██║   ██║
                   ██║   ██║   ██║██╔══██╗██╔══██╗██║   ██║
                   ██║   ╚██████╔╝██║  ██║██████╔╝╚██████╔╝
                   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚═════╝  ╚═════╝ 
            ");
            Console.WriteLine("Running {0}", GetVersion());
            Console.WriteLine();

            SetDefaultCulture(CultureInfo.InvariantCulture);

            // Register applicaton lifetime events
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            // Start services
            _pluginManager.LoadPlugins();
            _serverManager.StartServersAsync();

            _securityManager.InitAsync();
            _furnitureManager.InitAsync();
            _roomManager.InitAsync();

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called by the host application lifetime after the emulator started succesfully
        /// </summary>
        private void OnStarted() => _logger.LogInformation("Emulator started succesfully!");

        /// <summary>
        /// This method is called by the host application lifetime right before the emulator starts stopping
        /// Perform on-stopping activities here.
        /// This function is called before <see cref="StopAsync(CancellationToken)"/>
        /// </summary>
        private void OnStopping() => _logger.LogInformation("OnStopping has been called.");

        /// <summary>
        /// This method is called by the .NET Generic Host.
        /// See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0 for more information.
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down. Disposing services...");

            // Todo: Dispose all services here
            _roomManager.DisposeAsync();
            _furnitureManager.DisposeAsync();
            _roomManager.DisposeAsync();
            _playerManager.DisposeAsync();
            _serverManager.ShutdownServersAsync();

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called by the host application lifetime after the emulator stopped succesfully
        /// </summary>
        private void OnStopped() => _logger.LogInformation("{Emulator} shut down gracefully.", GetVersion());

        private void SetDefaultCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public string GetVersion()
        {
            var sb = new StringBuilder();
            sb.Append("Tu");
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                sb.Append("r");
            }
            sb.Append($"bo Emulator {MAJOR}.{MINOR}.{PATCH}");
            return sb.ToString();
        }
    }
}
