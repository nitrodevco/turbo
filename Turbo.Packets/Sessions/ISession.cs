using Turbo.Core.Players;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Revisions;
﻿using Turbo.Packets.Composers;
using Turbo.Core;
using System;

namespace Turbo.Packets.Sessions
{
    public interface ISession : IAsyncDisposable
    {
        public IPlayer Player { get; set; }
        public string IPAddress { get; set; }
        public long LastPongTimestamp { get; set; }
        public IRevision Revision { get; set; }
        public ISessionPlayer SessionPlayer { get; }
        public bool SetSessionPlayer(ISessionPlayer sessionPlayer);
        public string IPAddress { get; }
        public void Disconnect();
        public ISession Send(IComposer composer);
        public ISession SendQueue(IComposer composer);
        public void Flush();
    }
}
