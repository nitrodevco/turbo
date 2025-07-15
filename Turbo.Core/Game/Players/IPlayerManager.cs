using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Inventory;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Players;

public interface IPlayerManager : IComponent
{
    public Task<IPlayer> GetPlayerById(int id);
    public Task<IPlayer> GetPlayerByUsername(string username);
    public Task<IPlayer> CreatePlayer(int id, ISession session);
    public Task RemovePlayer(int id);
    public Task RemoveAllPlayers();
    public void ClearPlayerRoomStatus(IPlayer player);
    public Task<string> GetPlayerName(int playerId);
    public Task<IList<IPlayerBadge>> GetPlayerActiveBadges(int playerId);
    public int GetPendingRoomId(int userId);
    public void SetPendingRoomId(int userId, int roomId, bool approved = false);
    public void ClearPendingRoomId(int userId);
    public void ClearPendingDoorbell(IPlayer player);
    public void ClearRoomStatus(IPlayer player);
    public Task OpenRoom(IPlayer player, int roomId, string password = null, bool skipState = false, IPoint location = null);
    public Task EnterRoom(IPlayer player);
}