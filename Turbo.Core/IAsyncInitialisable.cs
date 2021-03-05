using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core
{
    public interface IAsyncInitialisable
    {
        public ValueTask InitAsync();
    }
}
