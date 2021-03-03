using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
        private readonly IPacketMessageHub _messageHub;
        private readonly ISessionManager _sessionManager;
        private readonly IRevisionManager _revisionManager;
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger<GameMessageHandler> _logger;

        public GameMessageHandler(IPacketMessageHub messageHub,
            ISessionManager sessionManager,
            IRevisionManager revisionManager,
            ISessionFactory sessionFactory,
            ILogger<GameMessageHandler> logger)
        {
            _messageHub = messageHub;
            _sessionManager = sessionManager;
            _revisionManager = revisionManager;
            _sessionFactory = sessionFactory;
            _logger = logger;
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
                    _logger.LogDebug($"Received {msg.Header}:{parser.GetType().Name}");
                    await parser.HandleAsync(session, msg, _messageHub);
                }
                else
                {
                    _logger.LogDebug($"No matching parser found for message {msg.Header}:{msg}");
                }
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (exception is IOException) return;
            _logger.LogError(exception.Message);
        }
    }
}
