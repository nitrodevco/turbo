using DotNetty.Transport.Channels;
using System;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Packets;
using Turbo.Packets.Incoming;
using Turbo.Packets.Parsers;
using Turbo.Packets.Revisions;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Game.Handler
{
    class GameMessageHandler : SimpleChannelInboundHandler<IClientPacket>
    {
        private IPacketMessageHub _messageHub;
        private ISessionManager _sessionManager;
        private IRevisionManager _revisionManager;
        private ISessionFactory _sessionFactory;

        public GameMessageHandler(IPacketMessageHub messageHub,
            ISessionManager sessionManager,
            IRevisionManager revisionManager,
            ISessionFactory sessionFactory)
        {
            _messageHub = messageHub;
            _sessionManager = sessionManager;
            _revisionManager = revisionManager;
            _sessionFactory = sessionFactory;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _sessionManager.TryRegisterSession(context.Channel.Id,
                _sessionFactory.Create(context, _revisionManager.DefaultRevision));
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _sessionManager.DisconnectSession(context.Channel.Id);
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, IClientPacket msg)
        {
            if (_sessionManager.TryGetSession(ctx.Channel.Id, out ISession session))
            {
                if (session.Revision.Parsers.TryGetValue(msg.Header, out IParser parser))
                {
                    Console.WriteLine("Received " + msg.Header + " : " + parser.GetType().Name);
                    await parser.HandleAsync(session, msg, _messageHub);
                }
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            // log the error
        }
    }
}
