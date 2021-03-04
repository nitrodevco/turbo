using System;
using System.Collections.Generic;
using Turbo.Packets.Parsers;
using Turbo.Packets.Serializers;

namespace Turbo.Packets.Revisions
{
    public interface IRevision
    {
        public string Revision { get; }
        public IDictionary<int, IParser> Parsers { get; }
        public IDictionary<Type, ISerializer> Serializers { get; }
    }
}
