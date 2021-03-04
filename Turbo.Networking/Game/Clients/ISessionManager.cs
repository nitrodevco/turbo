using DotNetty.Transport.Channels;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Clients
{
    public interface ISessionManager
    {
        public bool TryGetSession(IChannelId id, out ISession session);
        public bool TryRegisterSession(IChannelId id, in ISession session);
        public void DisconnectSession(IChannelId id);
    }
}
