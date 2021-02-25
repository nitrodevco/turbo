using Turbo.Core.Players;
using Turbo.Packets.Composers;

namespace Turbo.Packets.Sessions
{
    public interface ISession
    {
        public IPlayer Player { get; }
        public string IPAddress { get; }
        public void Disconnect();
        public ISession Send(IComposer composer);
        public ISession SendQueue(IComposer composer);
    }
}
