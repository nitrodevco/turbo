using System.Threading.Tasks;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Parsers
{
    public abstract class AbstractParser<T> : IParser
        where T : IMessageEvent
    {
        public async virtual Task HandleAsync(ISession session, IClientPacket message, IPacketMessageHub hub)
        {
            T messageEvent = (T)Parse(message);
            await hub.PublishAsync(messageEvent, session);
        }

        abstract public IMessageEvent Parse(IClientPacket packet);
    }
}
