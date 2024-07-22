using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms.Managers;

public interface IRoomUserManager : IComponent
{
    public IRoomObjectContainer<IRoomObjectAvatar> AvatarObjects { get; }
    public IRoomObjectAvatar GetRoomObjectByUserId(int userId);
    public IRoomObjectAvatar GetRoomObjectByUsername(string username);
    public IRoomObjectAvatar AddRoomObject(IRoomObjectAvatar avatarObject, IPoint location = null);
    public IRoomObjectAvatar CreateRoomObjectAndAssign(IRoomObjectAvatarHolder userHolder, IPoint location = null);
    public void RemoveRoomObject(IRoomObjectAvatar avatarObject);
    public IRoomObjectAvatar EnterRoom(IPlayer player, IPoint location = null);
    public void SendComposer(IComposer composer);
}