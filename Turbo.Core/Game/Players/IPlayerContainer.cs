using System.Threading.Tasks;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerContainer
    {
        public ValueTask RemovePlayer(int id);
        public ValueTask RemoveAllPlayers();
        public void ClearPlayerRoomStatus(IPlayer player);
    }
}
