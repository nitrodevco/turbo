using DotNetty.Transport.Channels;
using Turbo.Packets.Revisions;
using Turbo.Packets.Sessions;

namespace Turbo.Networking.Game.Clients
{
    public interface ISessionFactory
    {
        public ISession Create(IChannelHandlerContext context, IRevision initialRevision);
    }
}
