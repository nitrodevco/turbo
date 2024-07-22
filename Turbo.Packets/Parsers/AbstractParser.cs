using System.Threading.Tasks;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets;
using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Parsers;

public abstract class AbstractParser<T> : IParser
    where T : IMessageEvent
{
    public virtual async Task HandleAsync(ISession session, IClientPacket message, IPacketMessageHub hub)
    {
        var messageEvent = (T)Parse(message);
        await hub.PublishAsync(messageEvent, session);
    }

    public abstract IMessageEvent Parse(IClientPacket packet);
}