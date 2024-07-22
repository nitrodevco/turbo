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

namespace Turbo.Networking.REST;

public class RestServer : IRestServer
{
    private readonly IEmulatorConfig _config;
    private readonly INetworkEventLoopGroup _eventLoopGroup;
    private readonly ILogger<RestServer> _logger;
    private readonly IServiceProvider _provider;

    protected readonly ServerBootstrap _serverBootstrap;

    public RestServer(
        ILogger<RestServer> logger,
        IEmulatorConfig config,
        INetworkEventLoopGroup eventLoopGroup,
        IServiceProvider provider)
    {
        _logger = logger;
        _config = config;
        _eventLoopGroup = eventLoopGroup;
        _provider = provider;

        Host = _config.RCONHost;
        Port = _config.RCONPort;

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
        _serverBootstrap.ChildHandler(new RestChannelInitializer(_provider));
    }

    public async Task ShutdownAsync()
    {
        await ServerChannel.CloseAsync();
    }

    public async Task StartAsync()
    {
        ServerChannel = await _serverBootstrap.BindAsync(IPAddress.Parse(Host), Port);
        _logger.LogInformation("{Context} -> Listening on http://{Host}:{Port}", nameof(RestServer), Host, Port);
    }
}