using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Constants;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomSecurityManager : IComponent
{
    public IDictionary<int, DateTime> Bans { get; }
    public IList<int> Rights { get; }
    public bool IsStrictOwner(IRoomManipulator manipulator);
    public bool IsStrictOwner(int? playerId);
    public bool IsOwner(IRoomManipulator manipulator);
    public bool IsOwner(int? playerId);
    public bool IsPlayerBanned(IPlayer player);
    public RoomControllerLevel GetControllerLevel(IRoomManipulator manipulator);
    public void RefreshControllerLevel(IRoomObjectAvatar avatarObject);
    public void KickPlayer(IRoomManipulator manipulator, int playerId);
    public Task BanPlayerIdWithDuration(IRoomManipulator manipulator, int playerId, double durationMs);
    public Task AdjustRightsForPlayerId(IRoomManipulator manipulator, int playerId, bool flag);
    public Task RemoveAllRights(IRoomManipulator manipulator);
    public void SendOwnersComposer(IComposer composer);
    public void SendRightsComposer(IComposer composer);
    public bool CanKickPlayer(IRoomManipulator manipulator);
    public bool CanAdjustPlayerBan(IRoomManipulator manipulator, bool flag);
    public bool CanAdjustPlayerMute(IRoomManipulator manipulator, bool flag);
    public bool CanManipulateFurniture(IRoomManipulator manipulator, IRoomFurniture furniture);
    public bool CanPlaceFurniture(IRoomManipulator manipulator);
    public FurniturePickupType GetFurniturePickupType(IRoomManipulator manipulator, IRoomFurniture furniture);
}