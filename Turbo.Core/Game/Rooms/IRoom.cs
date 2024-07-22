using Microsoft.Extensions.Logging;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;

namespace Turbo.Core.Game.Rooms;

public interface IRoom : IComponent, ICyclable
{
    public ILogger<IRoom> Logger { get; }
    public IRoomManager RoomManager { get; }
    public IRoomDetails RoomDetails { get; }
    public IRoomCycleManager RoomCycleManager { get; }
    public IRoomSecurityManager RoomSecurityManager { get; }
    public IRoomFurnitureManager RoomFurnitureManager { get; }
    public IRoomUserManager RoomUserManager { get; }
    public IRoomChatManager RoomChatManager { get; }

    public IRoomModel RoomModel { get; }
    public IRoomMap RoomMap { get; }

    public int Id { get; }
    public bool IsGroupRoom { get; }

    public void TryDispose();
    public void CancelDispose();

    public void EnterRoom(IPlayer player, IPoint location = null);
    public void AddObserver(ISession session);
    public void RemoveObserver(ISession session);
    public void SendComposer(IComposer composer);
}