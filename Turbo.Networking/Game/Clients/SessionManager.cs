using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Threading;
using Turbo.Packets;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Clients
{
    public class SessionManager : ISessionManager
    {
        private readonly ConcurrentDictionary<IChannelId, ISession> _clients;
        private readonly Timer _timer;
        private readonly IPacketMessageHub _packetHub;

        public SessionManager(IPacketMessageHub packetHub)
        {
            _packetHub = packetHub;
            _clients = new ConcurrentDictionary<IChannelId, ISession>();

            _packetHub.Subscribe<PongMessage>(this, OnPongMessage);
            _timer = new Timer(new TimerCallback(ProcessPing), null, 0, 30000);
        }

        public bool TryGetSession(IChannelId id, out ISession session)
        {
            return this._clients.TryGetValue(id, out session);
        }

        public bool TryRegisterSession(IChannelId id, in ISession session)
        {
            return this._clients.TryAdd(id, session);
        }

        public void DisconnectSession(IChannelId id)
        {
            if(this._clients.TryRemove(id, out ISession session)) {
                session.Disconnect();
            }
        }

        /// <summary>
        /// Pings sessions every 30 seconds and disconnects sessions
        /// that have timed out for 60 seconds.
        /// </summary>
        /// <param name="state"></param>
        private void ProcessPing(object state)
        {
            foreach(ISession session in _clients.Values)
            {
                long timeNow = DateTimeOffset.Now.ToUnixTimeSeconds();

                if(timeNow - session.LastPongTimestamp > 60)
                {
                    session.Disconnect();
                    return;
                }

                session.Send(new PingMessage());
            }
        }

        public static void OnPongMessage(PongMessage message, ISession session)
        {
            session.LastPongTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }
}
