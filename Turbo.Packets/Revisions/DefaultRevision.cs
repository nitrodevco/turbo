using System;
using System.Collections.Generic;
using Turbo.Packets.Headers;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Parsers;
using Turbo.Packets.Parsers.Handshake;

namespace Turbo.Packets.Revisions
{
    class DefaultRevision : IRevision
    {
        public string Revision { get; }
        public IDictionary<int, IParser> Parsers { get; }
        public IDictionary<Type, ISerializer> Serializers { get; }

        public DefaultRevision()
        {
            Revision = "PRODUCTION-201611291003-338511768";
            Parsers = new Dictionary<int, IParser>();
            Serializers = new Dictionary<Type, ISerializer>();
            this.RegisterParsers();
        }

        private void RegisterParsers()
        {
            Parsers.Add(DefaultIncoming.ClientHello, new ClientHelloParser());
            Parsers.Add(DefaultIncoming.SSOTicket, new SSOTicketParser());
        }
    }
}
