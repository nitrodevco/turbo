using System.Collections.Generic;

namespace Turbo.Packets.Revisions
{
    public interface IRevisionManager
    {
        public IRevision DefaultRevision { get; set; }
        public IDictionary<string, IRevision> Revisions { get; }
    }
}
