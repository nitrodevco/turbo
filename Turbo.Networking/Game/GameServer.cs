using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Turbo.Core.Configuration;
using Turbo.Networking.EventLoop;

namespace Turbo.Networking.Game;

public class GameServer : IGameServer
{
    private readonly IEmulatorConfig _config;
    private readonly INetworkEventLoopGroup _eventLoopGroup;
    private readonly ILogger<GameServer> _logger;
    private readonly IServiceProvider _provider;

    protected readonly ServerBootstrap _serverBootstrap;

    public GameServer(
        ILogger<GameServer> logger,
        IEmulatorConfig config,
        INetworkEventLoopGroup eventLoopGroup,
        IServiceProvider provider)
    {
        _logger = logger;
        _config = config;
        _eventLoopGroup = eventLoopGroup;
        _provider = provider;

        Host = _config.GameHost;
        Port = _config.GameTCPPort;

        _serverBootstrap = new ServerBootstrap();
        InitializeBoostrap();
    }

    protected IChannel ServerChannel { get; private set; }

    public string Host { get; }
    public int Port { get; }

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
        _serverBootstrap.ChildHandler(new GameChannelInitializer(_provider));
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