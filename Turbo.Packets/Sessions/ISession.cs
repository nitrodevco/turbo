using Turbo.Core.Players;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Revisions;

namespace Turbo.Packets.Sessions
{
    public interface ISession
    {
        public IPlayer Player { get; set; }
        public string IPAddress { get; set; }
        public IRevision Revision { get; set; }
        public void Disconnect();
        public ISession Send(IComposer composer);
        public ISession SendQueue(IComposer composer);
        public void Flush();
    }
}
