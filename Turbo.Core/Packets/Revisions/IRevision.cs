using System;
using System.Collections.Generic;
using Turbo.Core.Packets.Messages;

namespace Turbo.Core.Packets.Revisions;

public interface IRevision
{
    public string Revision { get; }
    public IDictionary<int, IParser> Parsers { get; }
    public IDictionary<Type, ISerializer> Serializers { get; }
}