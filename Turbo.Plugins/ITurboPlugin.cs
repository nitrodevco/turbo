using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Plugins
{
    public interface ITurboPlugin
    {
        public string PluginName { get; }
        public string PluginAuthor { get; }
    }
}
