using System;
using System.Collections.Generic;
using Turbo.Packets.Outgoing;
using Turbo.Packets.Parsers;

namespace Turbo.Packets.Revisions
{
    public interface IRevision
    {
        public string Revision { get; }
        public IDictionary<int, IParser> Parsers { get; }
        public IDictionary<Type, ISerializer> Serializers { get; }
    }
}
