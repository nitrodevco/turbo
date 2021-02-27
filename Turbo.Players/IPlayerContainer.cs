using System;
using System.Collections.Generic;
using System.Text;

namespace Turbo.Players
{
    public interface IPlayerContainer
    {
        public void RemovePlayer(int id);
        public void RemoveAllPlayers();
    }
}
