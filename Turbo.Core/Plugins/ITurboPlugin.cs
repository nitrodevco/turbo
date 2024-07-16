using System;
using System.Collections;
using System.Collections.Generic;

namespace Turbo.Core.Plugins
{
    public interface ITurboPlugin
    {
        public string PluginName { get; }
        public string PluginAuthor { get; }
    }
}
