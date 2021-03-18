﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.Packets.Messages;
using Turbo.Database.Entities.Room;
using Turbo.Packets.Outgoing.Navigator;
using Turbo.Packets.Outgoing.Room.Engine;
using Turbo.Rooms.Managers;
using Turbo.Rooms.Mapping;

namespace Turbo.Rooms
{
    public class Room : IRoom
    {
        public IRoomManager RoomManager { get; private set; }
        public ILogger<IRoom> Logger { get; private set; }
        private readonly IRoomObjectFactory _roomObjectFactory;

        public IRoomDetails RoomDetails { get; private set; }
        public IRoomModel RoomModel { get; private set; }
        public IRoomMap RoomMap { get; private set; }
        public IRoomCycleManager RoomCycleManager { get; private set; }
        public IRoomSecurityManager RoomSecurityManager { get; private set; }
        public IRoomFurnitureManager RoomFurnitureManager { get; private set; }
        public IRoomUserManager RoomUserManager { get; private set; }

        private readonly IList<ISession> _roomObservers;

        public bool IsDisposed { get; private set; }
        public bool IsDisposing { get; private set; }

        public Room(
            IRoomManager roomManager,
            IRoomObjectFactory roomObjectFactory,
            ILogger<IRoom> logger,
            RoomEntity roomEntity)
        {
            RoomManager = roomManager;
            Logger = logger;
            _roomObjectFactory = roomObjectFactory;

            RoomDetails = new RoomDetails(roomEntity);
            RoomCycleManager = new RoomCycleManager(this);
            RoomSecurityManager = new RoomSecurityManager(this);
            RoomFurnitureManager = new RoomFurnitureManager(this);
            RoomUserManager = new RoomUserManager(this);

            _roomObservers = new List<ISession>();
        }

        public async ValueTask InitAsync()
        {
            await LoadMapping();

            if (RoomSecurityManager != null) await RoomSecurityManager.InitAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.InitAsync();
            if (RoomUserManager != null) await RoomUserManager.InitAsync();

            Logger.LogInformation("Room loaded");
        }

        public async ValueTask DisposeAsync()
        {
            if (IsDisposing) return;

            IsDisposing = true;

            CancelDispose();

            if (RoomManager != null) await RoomManager.RemoveRoom(Id);

            if (RoomCycleManager != null) await RoomCycleManager.DisposeAsync();
            if (RoomUserManager != null) await RoomUserManager.DisposeAsync();
            if (RoomFurnitureManager != null) await RoomFurnitureManager.DisposeAsync();
            if (RoomSecurityManager != null) await RoomSecurityManager.DisposeAsync();

            Logger.LogInformation("Room disposed");
        }

        public void TryDispose()
        {
            if (IsDisposed || IsDisposing) return;

            // if dispose already scheduled, return

            // if has users, return

            // clear the users waiting at the door

            // schedule the dispose to run in 1 minute
        }

        public void CancelDispose()
        {
            // if dispose already scheduled, cancel it
        }

        private async Task LoadMapping()
        {
            if (RoomMap != null)
            {
                RoomMap.Dispose();

                RoomMap = null;
            }

            RoomModel = null;

            IRoomModel roomModel = RoomManager.GetModel(RoomDetails.ModelId);

            if ((roomModel == null) || (!roomModel.DidGenerate)) return;

            RoomModel = roomModel;
            RoomMap = new RoomMap(this);

            RoomMap.GenerateMap();
        }

        public void EnterRoom(IPlayer player)
        {
            if (player == null) return;

            player.Session.SendQueue(new HeightMapMessage
            {
                RoomModel = RoomModel,
                RoomMap = RoomMap
            });

            player.Session.SendQueue(new FloorHeightMapMessage
            {
                IsZoomedIn = true,
                WallHeight = 1,
                RoomModel = RoomModel
            });

            //player.Session.SendQueue(new RoomVisualizationSettingsMessage
            //{
            //    WallsHidden = false,
            //    FloorThickness = 1,
            //    WallThickness = 1
            ////});

            // send the paint

            // would be nice to send this from the navigator so we aren't duplicating code
            player.Session.SendQueue(new GetGuestRoomResultMessage
            {
                EnterRoom = false,
                Room = this,
                IsRoomForward = false,
                IsStaffPick = false,
                IsGroupMember = false,
                AllInRoomMuted = false,
                CanMute = false
            });

            player.Session.Flush();

            RoomUserManager.EnterRoom(_roomObjectFactory, player);

            // send furniture
            // refresh rights
            // apply muted from security

            player.Session.Flush();

            _roomObservers.Add(player.Session);

            // process wired triggers for entering room
        }

        public async Task Cycle()
        {
            await RoomCycleManager.RunCycles();
        }

        public async ValueTask SendComposer(IComposer composer)
        {
            foreach (ISession session in _roomObservers)
            {
                await session.Send(composer);
            }
        }

        public int Id => RoomDetails.Id;
    }
}
