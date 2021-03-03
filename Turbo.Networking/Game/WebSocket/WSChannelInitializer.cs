using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Networking.Game.WebSocket.Codec;
using Turbo.Packets;
using Turbo.Packets.Revisions;

namespace Turbo.Networking.Game.WebSocket
{
    public class WSChannelInitializer : ChannelInitializer<IChannel>
    {
        private IPacketMessageHub _hub;
        private ISessionManager _sessionManager;
        private IRevisionManager _revisionManager;
        private ISessionFactory _sessionFactory;

        public WSChannelInitializer(IPacketMessageHub hub, ISessionManager sessionManager, IRevisionManager revisionManager,
            ISessionFactory sessionFactory)
        {
            _hub = hub;
            _sessionManager = sessionManager;
            _revisionManager = revisionManager;
            _sessionFactory = sessionFactory;
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
                .AddLast("messageHandler", new GameMessageHandler(_hub, _sessionManager, _revisionManager, _sessionFactory));
        }
    }
}
