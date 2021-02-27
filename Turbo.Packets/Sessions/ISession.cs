using Turbo.Packets.Composers;
using Turbo.Core;
using System;

namespace Turbo.Packets.Sessions
{
    public interface ISession : IDisposable
    {
        public ISessionPlayer SessionPlayer { get; }
        public bool SetSessionPlayer(ISessionPlayer sessionPlayer);
        public string IPAddress { get; }
        public void Disconnect();
        public ISession Send(IComposer composer);
        public ISession SendQueue(IComposer composer);
    }
}
