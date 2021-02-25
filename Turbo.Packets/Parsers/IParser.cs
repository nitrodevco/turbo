using Turbo.Packets.Incoming;

namespace Turbo.Packets.Parsers
{
    public interface IParser<T> where T : IMessageEvent
    {
        T Parse(ClientPacket packet);
    }
}
