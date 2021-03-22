using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;

namespace Turbo.Networking.Clients
{
    public class SessionManager : ISessionManager
    {
        private readonly IPacketMessageHub _packetHub;
        private readonly ConcurrentDictionary<IChannelId, ISession> _clients;
        private const int _pingIntervalSeconds = 30;
        private long _lastPingSeconds = 0;

        public SessionManager(IPacketMessageHub packetHub)
        {
            _packetHub = packetHub;
            _clients = new ConcurrentDictionary<IChannelId, ISession>();

            _packetHub.Subscribe<PongMessage>(this, OnPongMessage);
        }

        public bool TryGetSession(IChannelId id, out ISession session)
        {
            return _clients.TryGetValue(id, out session);
        }

        public bool TryRegisterSession(IChannelId id, in ISession session)
        {
            return _clients.TryAdd(id, session);
        }

        public void DisconnectSession(IChannelId id)
        {
            if (_clients.TryRemove(id, out ISession session))
            {
                session.DisposeAsync();
            }
        }

        /// <summary>
        /// Pings sessions every 30 seconds and disconnects sessions
        /// that have timed out for 60 seconds.
        /// </summary>
        private void ProcessPing()
        {
            long timeNow = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (timeNow - _lastPingSeconds < _pingIntervalSeconds) return;

            foreach (ISession session in _clients.Values)
            {
                if (timeNow - session.LastPongTimestamp > 60)
                {
                    session.DisposeAsync();

                    continue;
                }

                session.Send(new PingMessage());
            }

            _lastPingSeconds = timeNow;
        }

        public static void OnPongMessage(PongMessage message, ISession session)
        {
            session.LastPongTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public Task Cycle()
        {
            ProcessPing();

            return Task.CompletedTask;
        }
    }
}
