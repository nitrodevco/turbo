using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Packets.Revisions;
using Turbo.Core.Security;
using Turbo.Networking.Extensions;

namespace Turbo.Networking.Game.Clients;

public class Session : ISession
{
    private readonly IChannelHandlerContext _channel;
    private readonly ILogger<Session> _logger;
    private readonly ConcurrentQueue<IClientPacket> pendingReadMessages = new();
    private readonly IPacketMessageHub _messageHub;

    public Session(
        IChannelHandlerContext channel,
        IRevision initialRevision,
        ILogger<Session> logger,
        IPacketMessageHub messageHub
    )
    {
        _channel = channel;
        _logger = logger;
        _messageHub = messageHub;

        Revision = initialRevision;
        LastPongTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public IChannel Channel => _channel.Channel;
    public IRevision Revision { get; set; }
    public IRc4Service Rc4 { get; set; }
    public IPlayer Player { get; private set; }

    public string IPAddress { get; }
    public long LastPongTimestamp { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (Player != null)
        {
            await Player.DisposeAsync();

            Player = null;
        }

        await _channel.CloseAsync();
    }

    public bool SetPlayer(IPlayer player)
    {
        if (Player != null && Player != player) return false;

        Player = player;

        return true;
    }

    public async Task Send(IComposer composer)
    {
        await Send(composer, false);
    }

    public async Task SendQueue(IComposer composer)
    {
        await Send(composer, true);
    }

    public void Flush()
    {
        _channel.Flush();
    }

    private bool TryAddMessage(IClientPacket messageEvent)
    {
        pendingReadMessages.Enqueue(messageEvent);
        messageEvent.Content.Retain();
        return true;
    }

    public void OnMessageReceived(IClientPacket messageEvent)
    {
        if (!TryAddMessage(messageEvent))
        {
            messageEvent.Content.ReleaseAll();
        }
    }

    public async Task HandleDecodedMessages()
    {
        while (true)
        {
            if (pendingReadMessages.IsEmpty)
            {
                break;
            }

            if (!pendingReadMessages.TryDequeue(out var msg)) continue;
            try
            {
                if (Revision.Parsers.TryGetValue(msg.Header, out var parser))
                {
                    _logger.LogInformation($"INCOMING [{parser.GetType().Name}] -> {msg.ToString()}"); //\n\t[{msg.Header}] -> {msg.ToString()}

                    await parser.HandleAsync(this, msg, _messageHub);
                }
                else
                {
                    _logger.LogWarning("No matching parser found for message {}", msg.Header);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling message");
            }
            finally
            {
                msg.Content.Release();
            }
        }
    }

    protected async Task Send(IComposer composer, bool queue)
    {
        if (!IsConnected()) return;

        if (Revision.Serializers.TryGetValue(composer.GetType(), out var serializer))
        {
            var packet = serializer.Serialize(_channel.Allocator.Buffer(), composer);

            _logger.LogInformation($"OUTGOING [{composer.GetType().Name}] -> {packet.ToString()}"); //\n\t[{packet.Header}] -> {packet.ToString()}

            try
            {
                if (queue) await _channel.WriteAsync(packet);
                else await _channel.WriteAndFlushAsync(packet);
            }

            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
        else
        {
            _logger.LogWarning($"No matching serializer found for message {composer.GetType().Name}");
        }
    }

    public bool IsConnected()
    {
        return _channel.Channel.Open;
    }
}