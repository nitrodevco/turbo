using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Revisions
{
    public class RevisionManager : IRevisionManager
    {
        public IDictionary<string, IRevision> Revisions { get; }
        public string DefaultRevision { get; set; }

        public RevisionManager()
        {
            this.Revisions = new Dictionary<string, IRevision>();
            this.DefaultRevision = "PRODUCTION-201611291003-338511768";
            this.Revisions.Add(DefaultRevision, new DefaultRevision());
            this.Revisions.Add("NITRO-0-4-0", new DefaultRevision());
        }
    }
}
