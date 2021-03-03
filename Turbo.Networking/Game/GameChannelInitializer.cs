using DotNetty.Transport.Channels;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Networking.Game.Codec;
using Turbo.Networking.Game.Handler;
using Turbo.Packets;
using Turbo.Packets.Revisions;

namespace Turbo.Networking.Game
{
    public class GameChannelInitializer : ChannelInitializer<IChannel>
    {
        private IPacketMessageHub _hub;
        private ISessionManager _sessionManager;
        private IRevisionManager _revisionManager;
        private ISessionFactory _sessionFactory;

        public GameChannelInitializer(IPacketMessageHub hub, ISessionManager sessionManager, IRevisionManager revisionManager,
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
                .AddLast("flashPolicy", new FlashPolicyHandler())
                .AddLast("frameEncoder", new FrameLengthFieldEncoder())
                .AddLast("frameDecoder", new FrameLengthFieldDecoder())
                .AddLast("gameEncoder", new GameEncoder())
                .AddLast("gameDecoder", new GameDecoder())
                .AddLast("messageHandler", new GameMessageHandler(_hub, _sessionManager, _revisionManager, _sessionFactory));
        }
    }
}
