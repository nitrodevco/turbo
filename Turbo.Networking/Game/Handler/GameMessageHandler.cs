using System;
using System.IO;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Messages;
using Turbo.Networking.Clients;
using Turbo.Networking.Game.Clients;
using Turbo.Packets.Revisions;

namespace Turbo.Networking.Game.Handler;

public class GameMessageHandler : SimpleChannelInboundHandler<IClientPacket>
{
    private readonly ILogger<GameMessageHandler> _logger;
    private readonly IPacketMessageHub _messageHub;
    private readonly IRevisionManager _revisionManager;
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionManager _sessionManager;

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
        if (!_sessionManager.TryGetSession(ctx.Channel.Id, out var session))
        {
            _logger.LogInformation("Session not found for {}", ctx.Channel.RemoteAddress);
            return;
        }

        if (session.Revision == null)
        {
            _logger.LogInformation("Session revision not set for {}", ctx.Channel.RemoteAddress);

            await session.DisposeAsync();

            return;
        }

        session.OnMessageReceived(msg);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        if (exception is IOException) return;
        _logger.LogError(exception.Message);
    }
}