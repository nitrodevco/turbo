using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Turbo.Core;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game;
using Turbo.Networking.Game.WebSocket;
using Turbo.Networking.REST;
using Turbo.Plugins;

namespace Turbo.Main
{
    public class TurboEmulator : IEmulator
    {
        public const int MAJOR = 0;
        public const int MINOR = 0;
        public const int PATCH = 0;

        private readonly ILogger<TurboEmulator> _logger;
        private readonly IPluginManager _pluginManager;
        private readonly INetworkEventLoopGroup _networkEventLoopGroup;
        private readonly IGameServer _gameServer;
        private readonly IWSGameServer _wsGameServer;
        private readonly IRestServer _restServer;

        public TurboEmulator(ILogger<TurboEmulator> logger, IPluginManager pluginManager, INetworkEventLoopGroup eventLoop, IGameServer gameServer, IWSGameServer wSGameServer, IRestServer restServer)
        {
            _logger = logger;
            _pluginManager = pluginManager;
            _networkEventLoopGroup = eventLoop;
            _gameServer = gameServer;
            _wsGameServer = wSGameServer;
            _restServer = restServer;
        }

        public Task Start()
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

            SetDefaultCulture(CultureInfo.InvariantCulture);

            _pluginManager.LoadPlugins();
            _gameServer.StartAsync().Wait();
            _wsGameServer.StartAsync().Wait();
            _restServer.StartAsync().Wait();

            Console.ReadLine();
            return Task.CompletedTask;
        }

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
            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                sb.Append("r");
            }
            sb.Append($"bo Emulator {MAJOR}.{MINOR}.{PATCH}");
            return sb.ToString();
        }
    }
}
