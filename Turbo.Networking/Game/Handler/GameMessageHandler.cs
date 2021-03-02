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

        public GameMessageHandler(IPacketMessageHub messageHub, ISessionManager sessionManager, IRevisionManager revisionManager)
        {
            _messageHub = messageHub;
            _sessionManager = sessionManager;
            _revisionManager = revisionManager;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            _sessionManager.TryRegisterSession(context.Channel.Id, new Session(context, _revisionManager.DefaultRevision));
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
           if(_sessionManager.TryGetSession(context.Channel.Id, out ISession session))
           {
                session.Disconnect();
           }
        }

        protected override async void ChannelRead0(IChannelHandlerContext ctx, IClientPacket msg)
        {
            // do the thing where we get parser for the message
            // then publish message data to listeners
            if (_sessionManager.TryGetSession(ctx.Channel.Id, out ISession session))
            {
                if(session.Revision.Parsers.TryGetValue(msg.Header, out IParser parser))
                {
                    Console.WriteLine("Received " + msg.Header + " : "  + parser.GetType().Name);
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
