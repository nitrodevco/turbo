using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Packets.Messages;
using Turbo.Core.Utilities;
using Turbo.Packets.Outgoing.Room.Action;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Object;
using Turbo.Rooms.Object.Logic.Avatar;

namespace Turbo.Rooms.Managers;

public class RoomUserManager : Component, IRoomUserManager
{
    private readonly IRoom _room;
    private readonly IRoomObjectFactory _roomObjectFactory;

    public RoomUserManager(
        IRoom room,
        IRoomObjectFactory roomObjectFactory)
    {
        _room = room;
        _roomObjectFactory = roomObjectFactory;

        AvatarObjects = new RoomObjectContainer<IRoomObjectAvatar>(RemoveRoomObject);
    }

    public IRoomObjectContainer<IRoomObjectAvatar> AvatarObjects { get; }

    public IRoomObjectAvatar GetRoomObjectByUserId(int userId)
    {
        return null;
    }

    public IRoomObjectAvatar GetRoomObjectByUsername(string username)
    {
        foreach (var obj in AvatarObjects.RoomObjects.Values)
            if (obj.RoomObjectHolder is IPlayer player &&
                player.Name.Equals(username, StringComparison.OrdinalIgnoreCase))
                return obj;

        return null;
    }

    public IRoomObjectAvatar AddRoomObject(IRoomObjectAvatar avatarObject, IPoint location = null)
    {
        if (avatarObject == null) return null;

        var existingRoomObject = AvatarObjects.GetRoomObject(avatarObject.Id);

        if (existingRoomObject != null)
        {
            avatarObject.Dispose();

            return null;
        }

        location ??= _room.RoomModel.DoorLocation.Clone();

        avatarObject.SetLocation(location);
        avatarObject.Location.SetRotation(location.Rotation);

        if (!avatarObject.Logic.OnReady())
        {
            avatarObject.Dispose();

            return null;
        }

        _room.RoomMap.AddAvatarObject(avatarObject);

        AvatarObjects.AddRoomObject(avatarObject);

        UpdateTotalUsers();

        avatarObject.Logic.CanWalk = true;

        return avatarObject;
    }

    public IRoomObjectAvatar CreateRoomObjectAndAssign(IRoomObjectAvatarHolder userHolder, IPoint location = null)
    {
        if (userHolder == null) return null;

        var logicType = "";

        switch (userHolder.Type)
        {
            case RoomObjectHolderType.User:
                logicType = "user";
                break;
            case RoomObjectHolderType.Pet:
                logicType = "pet";
                break;
            case RoomObjectHolderType.Bot:
                logicType = "bot";
                break;
            case RoomObjectHolderType.RentableBot:
                logicType = "rentablebot";
                break;
        }

        var roomObject =
            _roomObjectFactory.CreateAvatarObject(_room, AvatarObjects, AvatarObjects.GetNextId(), logicType);

        if (roomObject == null) return null;

        if (!userHolder.SetRoomObject(roomObject)) return null;

        return AddRoomObject(roomObject, location);
    }

    public void RemoveRoomObject(IRoomObjectAvatar avatarObject)
    {
        if (avatarObject == null || avatarObject.Disposed) return;

        AvatarObjects.RemoveRoomObject(avatarObject);

        _room.RoomMap.RemoveAvatarObject(avatarObject);

        // if the avatar object was playing a game, remove it from that game

        avatarObject.Dispose();

        UpdateTotalUsers();

        _room.TryDispose();
    }

    public void AddPlayerToRoom(IPlayer player)
    {
        if (IsDisposing || player == null) return;

        List<IRoomObjectAvatar> roomObjects = new();
        List<IComposer> composers = new();

        foreach (var existingAvatarObject in AvatarObjects.RoomObjects.Values)
        {
            roomObjects.Add(existingAvatarObject);

            if (existingAvatarObject.Logic is AvatarLogic avatarLogic)
            {
                var danceType = avatarLogic.DanceType;

                if (danceType > RoomObjectAvatarDanceType.None)
                    composers.Add(new DanceMessage
                    {
                        ObjectId = existingAvatarObject.Id,
                        DanceStyle = (int)danceType
                    });

                if (existingAvatarObject.Logic is PlayerLogic playerLogic)
                    if (playerLogic.IsIdle)
                        composers.Add(new SleepMessage
                        {
                            ObjectId = existingAvatarObject.Id,
                            Sleeping = playerLogic.IsIdle
                        });
            }
        }

        player.Session.SendQueue(new UsersMessage
        {
            RoomObjects = roomObjects
        });

        //player.Session.SendQueue(new UserUpdateMessage
        //{
        //    RoomObjects = roomObjects
        //});

        foreach (var composer in composers) player.Session.SendQueue(composer);

        player.Session.Flush();
    }

    public void SendComposer(IComposer composer)
    {
        _room.SendComposer(composer);
    }

    protected override async Task OnInit()
    {
    }

    protected override async Task OnDispose()
    {
        AvatarObjects.RemoveAllRoomObjects();
    }

    private void UpdateTotalUsers()
    {
        var totalUsers = 0;

        foreach (var roomObject in AvatarObjects.RoomObjects.Values)
        {
            if (roomObject.RoomObjectHolder is not IPlayer player) continue;

            // check if the player increases the total user count or not

            totalUsers++;
        }

        _room.RoomDetails.UsersNow = totalUsers;
    }
}