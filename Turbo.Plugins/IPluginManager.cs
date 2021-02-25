using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Plugins
{
    public interface IPluginManager
    {
        public abstract void LoadPlugins();
    }
}
