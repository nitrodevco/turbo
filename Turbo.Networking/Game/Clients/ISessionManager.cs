using DotNetty.Transport.Channels;
using Turbo.Core.Game;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Networking.Clients;

public interface ISessionManager : ICyclable
{
    public bool TryGetSession(IChannelId id, out ISession session);
    public bool TryRegisterSession(IChannelId id, in ISession session);
    public void DisconnectSession(IChannelId id);
}