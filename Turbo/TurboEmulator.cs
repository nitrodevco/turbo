using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Plugins;
using Turbo.Core.Security;
using Turbo.Core.Security.Authentication;
using Turbo.Networking;

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

        private Task _gameCycle;
        private bool _cycleStarted = false;
        private bool _cycleRunningNow = false;

        public TurboEmulator(
            IHostApplicationLifetime appLifetime,
            ILogger<TurboEmulator> logger,
            IPluginManager pluginManager,
            IServerManager serverManager,
            ISecurityManager securityManager,
            IFurnitureManager furnitureManager,
            IRoomManager roomManager,
            IPlayerManager playerManager,
            IAuthenticationService authenticationService)
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
        public async Task StartAsync(CancellationToken cancellationToken)
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
            await _serverManager.StartServersAsync();

            await _securityManager.InitAsync();
            await _furnitureManager.InitAsync();
            await _roomManager.InitAsync();

            StartGameCycle();
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
        private void OnStopping()
        {
            _cycleStarted = false;
            _logger.LogInformation("OnStopping has been called.");
        }

        /// <summary>
        /// This method is called by the .NET Generic Host.
        /// See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-5.0 for more information.
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Shutting down. Disposing services...");

            _cycleStarted = false;

            // Todo: Dispose all services here
            await _roomManager.DisposeAsync();
            await _furnitureManager.DisposeAsync();
            await _roomManager.DisposeAsync();
            await _playerManager.DisposeAsync();
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

        private void StartGameCycle()
        {
            _cycleStarted = true;

            _gameCycle = Task.Run(async () =>
            {
                while (_cycleStarted)
                {
                    _cycleRunningNow = true;

                    try
                    {
                        Task.WaitAll(
                            Task.Run(async () => await _roomManager.Cycle()),
                            Task.Run(async () => await _playerManager.Cycle())
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Exception caught! " + ex.ToString());
                    }

                    _cycleRunningNow = false;
                    await Task.Delay(500);
                }
            });
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
