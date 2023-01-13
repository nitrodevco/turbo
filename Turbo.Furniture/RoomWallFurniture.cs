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
    public class RoomWallFurniture : IRoomWallFurniture
    {
        public ILogger<IRoomWallFurniture> Logger { get; private set; }

        private readonly IRoomFurnitureManager _roomFurnitureManager;
        private readonly IFurnitureManager _furnitureManager;
        private readonly FurnitureEntity _furnitureEntity;

        public IFurnitureDefinition FurnitureDefinition { get; private set; }
        public IRoomObjectWall RoomObject { get; private set; }
        public string PlayerName { get; set; }

        private IRoom _room;
        private bool _isDisposing { get; set; }

        public RoomWallFurniture(
            ILogger<IRoomWallFurniture> logger,
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
                _furnitureEntity.WallPosition = RoomObject.WallLocation;

                if (RoomObject.Logic.StuffData != null)
                {
                    _furnitureEntity.StuffData = JsonSerializer.Serialize(RoomObject.Logic.StuffData, RoomObject.Logic.StuffData.GetType());
                }
            }

            _furnitureManager.StorageQueue.Add(_furnitureEntity);
        }

        public bool SetRoom(IRoom room)
        {
            if ((room == null) || ((_room != null) && (_room != room))) return false;

            _room = room;

            if (_furnitureEntity.RoomEntityId != room.Id)
            {
                _furnitureEntity.RoomEntityId = room.Id;

                Save();
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

        public bool SetRoomObject(IRoomObjectWall roomObject)
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

        public int Id => _furnitureEntity.Id;

        public string SavedWallLocation => _furnitureEntity.WallPosition;

        public RoomObjectHolderType Type => RoomObjectHolderType.Furniture;

        public string LogicType => FurnitureDefinition.Logic;

        public int PlayerId => _furnitureEntity.PlayerEntityId;
    }
}
