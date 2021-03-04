using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Turbo.Packets.Sessions;

namespace Turbo.Packets.Parsers
{
    public abstract class AbstractParser<T> : IParser
        where T : IMessageEvent
    {
        public async virtual Task HandleAsync(ISession session, IClientPacket message, IPacketMessageHub hub)
        {
            T messageEvent = (T) Parse(message);
            await hub.PublishAsync(messageEvent, session);
        }

        abstract public IMessageEvent Parse(IClientPacket packet);
    }
}
