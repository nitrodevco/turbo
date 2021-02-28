using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Packets.Revisions
{
    public interface IRevisionManager
    {
        public string DefaultRevision { get; set; }
        public IDictionary<string, IRevision> Revisions { get; }
    }
}
