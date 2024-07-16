using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Navigator
{
    public interface INavigatorTab
    {
        public int Id { get; }
        public string SearchCode { get; }
        public ITopLevelContext TopLevelContext { get; }
    }
}
