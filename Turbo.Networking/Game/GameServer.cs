using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;
using Microsoft.Extensions.Logging;
using Turbo.Core.Configuration;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using System.Net;
using Turbo.Networking.EventLoop;

namespace Turbo.Networking.Game
{
    public class GameServer : IGameServer
    {
        private readonly ILogger<GameServer> _logger;
        private readonly IEmulatorConfig _config;
        private readonly INetworkEventLoopGroup _eventLoopGroup;

        protected readonly ServerBootstrap _serverBootstrap;
        protected IChannel ServerChannel { get; private set; }

        public string Host { get; }
        public int Port { get; }

        public GameServer(
            ILogger<GameServer> logger,
            IEmulatorConfig config,
            INetworkEventLoopGroup eventLoopGroup)
        {
            _logger = logger;
            _config = config;
            _eventLoopGroup = eventLoopGroup;

            Host = _config.GameHost;
            Port = _config.GameTCPPort;

            _serverBootstrap = new ServerBootstrap();
            InitializeBoostrap();
        }

        public void InitializeBoostrap()
        {
            _serverBootstrap.Group(_eventLoopGroup.Group);
            _serverBootstrap.Channel<TcpServerSocketChannel>();
            _serverBootstrap.ChildOption(ChannelOption.TcpNodelay, true);
            _serverBootstrap.ChildOption(ChannelOption.SoKeepalive, true);
            _serverBootstrap.ChildOption(ChannelOption.SoReuseaddr, true);
            _serverBootstrap.ChildOption(ChannelOption.SoRcvbuf, 4096);
            _serverBootstrap.ChildOption(ChannelOption.RcvbufAllocator, new FixedRecvByteBufAllocator(4096));
            _serverBootstrap.ChildOption(ChannelOption.Allocator, new UnpooledByteBufferAllocator(false));
            _serverBootstrap.ChildHandler(new GameChannelInitializer());
        }

        public async Task StartAsync()
        {
            ServerChannel = await _serverBootstrap.BindAsync(IPAddress.Parse(Host), Port);
            _logger.LogInformation("{Context} -> Listening on {Host}:{Port}", nameof(GameServer), Host, Port);
        }

        public async Task ShutdownAsync()
        {
            await ServerChannel.CloseAsync();
        }
    }
}
