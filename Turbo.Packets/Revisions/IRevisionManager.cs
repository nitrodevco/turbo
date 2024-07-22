using System.Collections.Generic;
using Turbo.Core.Packets.Revisions;

namespace Turbo.Packets.Revisions;

public interface IRevisionManager
{
    public IRevision DefaultRevision { get; set; }
    public IDictionary<string, IRevision> Revisions { get; }
}