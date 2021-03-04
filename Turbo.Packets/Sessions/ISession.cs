using System.Threading.Tasks;
using Turbo.Core.Players;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Revisions;

namespace Turbo.Packets.Sessions
{
    public interface ISession
    {
        public IPlayer Player { get; set; }
        public string IPAddress { get; set; }
        public long LastPongTimestamp { get; set; }
        public IRevision Revision { get; set; }
        public Task Disconnect();
        public Task Send(IComposer composer);
        public Task SendQueue(IComposer composer);
        public void Flush();
    }
}
