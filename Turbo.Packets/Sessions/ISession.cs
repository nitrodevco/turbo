using System.Threading.Tasks;
using Turbo.Packets.Revisions;
using Turbo.Core;
using System;
using Turbo.Packets.Outgoing;

namespace Turbo.Packets.Sessions
{
    public interface ISession : IAsyncDisposable
    {
        public IRevision Revision { get; set; }
        public ISessionPlayer SessionPlayer { get; }
        public string IPAddress { get; }
        public long LastPongTimestamp { get; set; }
        public bool SetSessionPlayer(ISessionPlayer sessionPlayer);
        public Task Send(IComposer composer);
        public Task SendQueue(IComposer composer);
        public void Flush();
    }
}
