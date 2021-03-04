using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Players
{
    public interface IPlayerContainer
    {
        public ValueTask RemovePlayer(int id);
        public ValueTask RemoveAllPlayers();
    }
}
