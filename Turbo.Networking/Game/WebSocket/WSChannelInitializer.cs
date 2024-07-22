using System;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Packets;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Networking.Game.WebSocket.Codec;
using Turbo.Packets.Revisions;

namespace Turbo.Networking.Game.WebSocket;

public class WSChannelInitializer : ChannelInitializer<IChannel>
{
    private readonly IPacketMessageHub _hub;
    private readonly IServiceProvider _provider;
    private readonly IRevisionManager _revisionManager;
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionManager _sessionManager;

    public WSChannelInitializer(IServiceProvider provider)
    {
        _provider = provider;

        _hub = _provider.GetService<IPacketMessageHub>();
        _sessionManager = _provider.GetService<ISessionManager>();
        _revisionManager = _provider.GetService<IRevisionManager>();
        _sessionFactory = _provider.GetService<ISessionFactory>();
    }

    protected override void InitChannel(IChannel channel)
    {
        channel.Pipeline
            .AddLast("httpCodec", new HttpServerCodec())
            .AddLast("objectAggregator", new HttpObjectAggregator(65536))
            .AddLast("wsProtocolHandler", new WebSocketServerProtocolHandler("/", true))
            .AddLast("websocketCodec", new WebSocketCodec())
            .AddLast("frameEncoder", new FrameLengthFieldEncoder())
            .AddLast("frameDecoder", new FrameLengthFieldDecoder())
            .AddLast("gameEncoder", new GameEncoder())
            .AddLast("gameDecoder", new GameDecoder())
            .AddLast("messageHandler", new GameMessageHandler(
                _hub,
                _sessionManager,
                _revisionManager,
                _sessionFactory,
                _provider.GetService<ILogger<GameMessageHandler>>())
            );
    }
}