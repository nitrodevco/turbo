using System.Threading.Tasks;
using Turbo.Core.Networking.Game.Clients;

namespace Turbo.Core.Packets.Messages;

public interface IParser
{
    public IMessageEvent Parse(IClientPacket packet);
    public Task HandleAsync(ISession session, IClientPacket message, IPacketMessageHub hub);
}