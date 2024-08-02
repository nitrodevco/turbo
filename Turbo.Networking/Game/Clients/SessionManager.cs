using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Networking.Clients;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Incoming.Tracking;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Outgoing.Tracking;

namespace Turbo.Networking.Game.Clients;

public class SessionManager : ISessionManager
{
    private readonly ConcurrentDictionary<IChannelId, ISession> _clients;
    private const int _pingIntervalSeconds = 60;
    private long _lastPingSeconds;

    public SessionManager(IPacketMessageHub packetHub)
    {
        _clients = new ConcurrentDictionary<IChannelId, ISession>();

        packetHub.Subscribe<PongMessage>(this, OnPongMessage);
    }

    public bool TryGetSession(IChannelId id, out ISession session)
    {
        return _clients.TryGetValue(id, out session);
    }

    public bool TryRegisterSession(IChannelId id, in ISession session)
    {
        return _clients.TryAdd(id, session);
    }

    public async void DisconnectSession(IChannelId id)
    {
        if (!_clients.TryRemove(id, out var session)) return;

        await session.DisposeAsync();
    }

    /// <summary>
    /// Pings sessions every 30 seconds and disconnects sessions
    /// that have timed out for 60 seconds.
    /// </summary>
    private async Task ProcessPing()
    {
        var timeNow = DateTimeOffset.Now.ToUnixTimeSeconds();

        if (timeNow - _lastPingSeconds < _pingIntervalSeconds) return;

        var tasks = new ConcurrentBag<Task>();

        foreach (var session in _clients.Values)
        {
            if (timeNow - session.LastPongTimestamp > 60)
            {
                tasks.Add(session.DisposeAsync().AsTask());

                continue;
            }

            await session.Send(new PingMessage());
        }

        await Task.WhenAll(tasks);

        _lastPingSeconds = timeNow;
    }

    private void OnPongMessage(PongMessage message, ISession session)
    {
        session.LastPongTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    private async Task Process()
    {
        var tasks = new ConcurrentBag<Task>();
        
        foreach (var session in _clients.Values)
        {
            if (session == null) continue;

            try
            {
                tasks.Add(session.HandleDecodedMessages());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating task for session: {ex}");
            }
        }
        
        await Task.WhenAll(tasks);
    }

    public async Task Cycle()
    {
        try
        {
            await ProcessPing();
            await Process();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}