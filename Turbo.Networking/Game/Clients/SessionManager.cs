using DotNetty.Transport.Channels;
using System.Collections.Concurrent;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Clients
{
    public class SessionManager : ISessionManager
    {
        private ConcurrentDictionary<IChannelId, ISession> _clients;

        public SessionManager()
        {
            this._clients = new ConcurrentDictionary<IChannelId, ISession>();
        }

        public bool TryGetSession(IChannelId id, out ISession session)
        {
            return this._clients.TryGetValue(id, out session);
        }

        public bool TryRegisterSession(IChannelId id, in ISession session)
        {
            return this._clients.TryAdd(id, session);
        }
    }
}
