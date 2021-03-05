using System.Threading.Tasks;
using Turbo.Packets.Incoming;
using Turbo.Packets.Sessions;

namespace Turbo.Packets.Parsers
{
    public interface IParser
    {
        public IMessageEvent Parse(IClientPacket packet);
        public Task HandleAsync(ISession session, IClientPacket message, IPacketMessageHub hub);
    }
}
