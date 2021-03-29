using Microsoft.Extensions.Logging;
using System.Text.Json;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture
{
    public class Furniture : IFurniture
    {
        public ILogger<IFurniture> Logger { get; private set; }

        private readonly IFurnitureContainer _furnitureContainer;
        private readonly IFurnitureManager _furnitureManager;
        private readonly FurnitureEntity _furnitureEntity;

        public IFurnitureDefinition FurnitureDefinition;
        public IRoomObject RoomObject { get; private set; }
        public string PlayerName { get; set; }

        private IRoom _room;
        private bool _isDisposing { get; set; }

        public Furniture(
            ILogger<IFurniture> logger,
            IFurnitureContainer furnitureContainer,
            IFurnitureManager furnitureManager,
            FurnitureEntity furnitureEntity,
            IFurnitureDefinition furnitureDefinition)
        {
            Logger = logger;
            _furnitureContainer = furnitureContainer;
            _furnitureManager = furnitureManager;
            _furnitureEntity = furnitureEntity;

            FurnitureDefinition = furnitureDefinition;
        }

        public void Dispose()
        {
            if (_isDisposing) return;

            _isDisposing = true;

            ClearRoomObject();

            if(_furnitureContainer != null)
            {
                _furnitureContainer.RemoveFurniture(Id);
            }

            Save();

            Logger.LogInformation("Furniture disposed");
        }

        public void Save()
        {
            if(RoomObject != null)
            {
                _furnitureEntity.X = RoomObject.Location.X;
                _furnitureEntity.Y = RoomObject.Location.Y;
                _furnitureEntity.Z = RoomObject.Location.Z;
                _furnitureEntity.Rotation = RoomObject.Location.Rotation;

                if(RoomObject.Logic is IFurnitureLogic furnitureLogic)
                {
                    _furnitureEntity.StuffData = JsonSerializer.Serialize(furnitureLogic.StuffData);
                }
            }

            // waits for skelly :rolling_eyes:
        }

        public bool SetRoom(IRoom room)
        {
            if ((room == null) || ((_room != null) && (_room != room))) return false;

            _room = room;

            if(_furnitureEntity.RoomEntityId != room.Id)
            {
                _furnitureEntity.RoomEntityId = room.Id;

                Save();
            }

            return true;
        }

        public bool SetRoomObject(IRoomObject roomObject)
        {
            ClearRoomObject();

            if ((roomObject == null) || !roomObject.SetHolder(this)) return false;

            if (roomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                furnitureLogic.Setup(FurnitureDefinition, _furnitureEntity.StuffData);
            }

            RoomObject = roomObject;

            return true;
        }

        public void ClearRoomObject()
        {
            if (RoomObject != null)
            {
                RoomObject.Dispose();

                RoomObject = null;
            }
        }

        public int Id => _furnitureEntity.Id;

        public string Type => "furniture";

        public string LogicType => FurnitureDefinition.Logic;

        public int PlayerId => _furnitureEntity.PlayerEntityId;
    }
}
