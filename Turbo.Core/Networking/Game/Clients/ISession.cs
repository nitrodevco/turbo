using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Turbo.Core.Game.Players;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Packets.Revisions;
using Turbo.Core.Security;

namespace Turbo.Core.Networking.Game.Clients
{
    public interface ISession : IAsyncDisposable
    {
        public IChannel Channel { get; }
        
        public IRevision Revision { get; set; }
        public IRc4Service Rc4 { get; set; }
        public IPlayer Player { get; }
        public string IPAddress { get; }
        public long LastPongTimestamp { get; set; }
        public bool SetPlayer(IPlayer player);
        public Task Send(IComposer composer);
        public Task SendQueue(IComposer composer);
        public void Flush();
    }
}
