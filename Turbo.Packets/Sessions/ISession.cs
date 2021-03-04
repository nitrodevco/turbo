using System.Threading.Tasks;
using Turbo.Core.Players;
using Turbo.Packets.Revisions;
using Turbo.Core;
using System;

namespace Turbo.Packets.Sessions
{
    public interface ISession : IAsyncDisposable
    {
        public IRevision Revision { get; set; }
        public ISessionPlayer SessionPlayer { get; }
        public string IPAddress { get; }
        public long LastPongTimestamp { get; set; }
        public bool SetSessionPlayer(ISessionPlayer sessionPlayer);
        public ISession Send(IComposer composer);
        public ISession SendQueue(IComposer composer);
        public void Flush();
    }
}
