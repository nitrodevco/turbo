using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;

namespace Turbo.Core.Game.Navigator
{
    public interface INavigatorManager : IAsyncInitialisable, IAsyncDisposable
    {
        public Task EnterRoom(IPlayer player, int roomId, string password = null, bool skipState = false);
    }
}
