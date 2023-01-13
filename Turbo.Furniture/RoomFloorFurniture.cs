using System;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Managers;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Constants;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Game.Players;

namespace Turbo.Furniture
{
    public class RoomFloorFurniture : IRoomFloorFurniture
    {
        public ILogger<IRoomFloorFurniture> Logger { get; private set; }

        private readonly IRoomFurnitureManager _roomFurnitureManager;
        private readonly IFurnitureManager _furnitureManager;
        private readonly FurnitureEntity _furnitureEntity;

        public IFurnitureDefinition FurnitureDefinition { get; private set; }
        public IRoomObjectFloor RoomObject { get; private set; }
        public string PlayerName { get; set; }

        private IRoom _room;
        private bool _isDisposing { get; set; }

        public RoomFloorFurniture(
            ILogger<IRoomFloorFurniture> logger,
            IRoomFurnitureManager roomFurnitureManager,
            IFurnitureManager furnitureManager,
            FurnitureEntity furnitureEntity,
            IFurnitureDefinition furnitureDefinition)
        {
            Logger = logger;
            _roomFurnitureManager = roomFurnitureManager;
            _furnitureManager = furnitureManager;
            _furnitureEntity = furnitureEntity;

            FurnitureDefinition = furnitureDefinition;
        }

        public void Dispose()
        {
            if (_isDisposing) return;

            _isDisposing = true;

            ClearRoomObject();

            if (_furnitureEntity != null) _furnitureManager.StorageQueue.SaveNow(_furnitureEntity);
        }

        public void Save()
        {
            if (RoomObject != null)
            {
                _furnitureEntity.X = RoomObject.Location.X;
                _furnitureEntity.Y = RoomObject.Location.Y;
                _furnitureEntity.Z = RoomObject.Location.Z;
                _furnitureEntity.Rotation = RoomObject.Location.Rotation;

                if (RoomObject.Logic.StuffData != null)
                {
                    _furnitureEntity.StuffData = JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
                }

                if (RoomObject.Logic is IFurnitureWiredLogic wiredLogic)
                {
                    if (wiredLogic.WiredData != null)
                    {
                        _furnitureEntity.WiredData = JsonSerializer.Serialize(wiredLogic.WiredData, wiredLogic.WiredData.GetType());
                    }
                }
            }

            _furnitureManager.StorageQueue.Add(_furnitureEntity);
        }

        public bool SetRoom(IRoom room)
        {
            if (room == null)
            {
                if (_furnitureEntity.RoomEntityId != null)
                {
                    _furnitureEntity.RoomEntityId = null;

                    Save();
                }
            }
            else
            {
                _room = room;

                if (_furnitureEntity.RoomEntityId != room.Id)
                {
                    _furnitureEntity.RoomEntityId = room.Id;

                    Save();
                }
            }

            return true;
        }

        public void ClearRoom()
        {
            if (_furnitureEntity.RoomEntityId != null)
            {
                _furnitureEntity.RoomEntityId = null;

                Save();
            }
        }

        public bool SetRoomObject(IRoomObjectFloor roomObject)
        {
            ClearRoomObject();

            if ((roomObject == null) || !roomObject.SetHolder(this)) return false;

            RoomObject = roomObject;

            return true;
        }

        public async Task<bool> SetupRoomObject()
        {
            if (RoomObject == null) return false;

            if (!await RoomObject.Logic.Setup(FurnitureDefinition, _furnitureEntity.StuffData)) return false;

            if (RoomObject.Logic is IFurnitureWiredLogic wiredLogic)
            {
                wiredLogic.SetupWiredData(_furnitureEntity.WiredData);
            }

            return true;
        }

        public void ClearRoomObject()
        {
            if (RoomObject != null)
            {
                Save();

                RoomObject.Dispose();

                RoomObject = null;
            }
        }

        public bool SetPlayer(IPlayer player)
        {
            if (player == null) return false;

            return SetPlayer(player.Id, player.Name);
        }

        public bool SetPlayer(int playerId, string playerName = "")
        {
            if (playerId <= 0) return false;

            if (_furnitureEntity.PlayerEntityId != playerId)
            {
                _furnitureEntity.PlayerEntityId = playerId;

                Save();
            }

            if (playerName.Length > 0) PlayerName = playerName;

            return true;
        }

        public async Task<TeleportPairingDto> GetTeleportPairing()
        {
            return await _furnitureManager.GetTeleportPairing(Id);
        }

        public int Id => _furnitureEntity.Id;

        public int SavedX => _furnitureEntity.X;

        public int SavedY => _furnitureEntity.Y;

        public double SavedZ => _furnitureEntity.Z;

        public Rotation SavedRotation => _furnitureEntity.Rotation;

        public RoomObjectHolderType Type => RoomObjectHolderType.Furniture;

        public string LogicType => FurnitureDefinition.Logic;

        public int PlayerId => _furnitureEntity.PlayerEntityId;
    }
}
