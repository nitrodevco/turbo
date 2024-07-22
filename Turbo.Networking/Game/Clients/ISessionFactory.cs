using DotNetty.Transport.Channels;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Revisions;

namespace Turbo.Networking.Game.Clients;

public interface ISessionFactory
{
    public ISession Create(IChannelHandlerContext context, IRevision initialRevision);
}