using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Turbo.Core.Configuration;
using Turbo.Networking.Clients;
using Turbo.Networking.EventLoop;
using Turbo.Networking.Game.Clients;
using Turbo.Packets;
using Turbo.Packets.Revisions;

namespace Turbo.Networking.Game.WebSocket
{
    public class WSGameServer : IWSGameServer
    {
        private readonly ILogger<WSGameServer> _logger;
        private readonly IEmulatorConfig _config;
        private readonly INetworkEventLoopGroup _eventLoopGroup;
        private readonly IPacketMessageHub _messageHub;
        private readonly ISessionManager _sessionManager;
        private readonly IRevisionManager _revisionManager;
        private readonly ISessionFactory _sessionFactory;

        protected readonly ServerBootstrap _serverBootstrap;
        protected IChannel ServerChannel { get; private set; }

        public string Host { get; }

        public int Port { get; }

        public WSGameServer(ILogger<WSGameServer> logger,
            IEmulatorConfig config,
            INetworkEventLoopGroup eventLoopGroup,
            IPacketMessageHub hub, ISessionManager sessionManager, IRevisionManager revisionManager, ISessionFactory sessionFactory)
        {
            _logger = logger;
            _config = config;
            _eventLoopGroup = eventLoopGroup;
            _messageHub = hub;
            _sessionManager = sessionManager;
            _revisionManager = revisionManager;
            _sessionFactory = sessionFactory;

            Host = _config.GameHost;
            Port = _config.GameWSPort;

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
            _serverBootstrap.ChildHandler(new WSChannelInitializer(_messageHub, _sessionManager, _revisionManager, _sessionFactory));
        }

        public async Task StartAsync()
        {
            ServerChannel = await _serverBootstrap.BindAsync(IPAddress.Parse(Host), Port);
            _logger.LogInformation("{Context} -> Listening on ws://{Host}:{Port}", nameof(WSGameServer), Host, Port);
        }

        public async Task ShutdownAsync()
        {
            await ServerChannel.CloseAsync();
        }
    }
}
