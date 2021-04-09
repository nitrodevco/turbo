using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Object.Logic;
using Turbo.Core.Game.Rooms.Object.Logic.Wired;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture
{
    public class Furniture : IFurniture
    {
        public ILogger<IFurniture> Logger { get; private set; }

        private readonly IFurnitureContainer _furnitureContainer;
        private readonly IFurnitureManager _furnitureManager;
        private readonly FurnitureEntity _furnitureEntity;

        public IFurnitureDefinition FurnitureDefinition { get; private set; }
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

            if (_furnitureContainer != null)
            {
                _furnitureContainer.RemoveFurniture(Id);
            }

            Logger.LogInformation("Furniture disposed");
        }

        public void Save()
        {
            if (RoomObject != null)
            {
                _furnitureEntity.X = RoomObject.Location.X;
                _furnitureEntity.Y = RoomObject.Location.Y;
                _furnitureEntity.Z = RoomObject.Location.Z;
                _furnitureEntity.Rotation = RoomObject.Location.Rotation;

                if (RoomObject.Logic is IFurnitureLogic furnitureLogic)
                {
                    if(furnitureLogic.StuffData != null)
                    {
                        _furnitureEntity.StuffData = JsonSerializer.Serialize(furnitureLogic.StuffData, furnitureLogic.StuffData.GetType());
                    }

                    if(furnitureLogic is IFurnitureWiredLogic wiredLogic)
                    {
                        if(wiredLogic.WiredData != null)
                        {
                            _furnitureEntity.WiredData = JsonSerializer.Serialize(wiredLogic.WiredData, wiredLogic.WiredData.GetType());
                        }
                    }
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

        public bool SetRoomObject(IRoomObject roomObject)
        {
            ClearRoomObject();

            if ((roomObject == null) || !roomObject.SetHolder(this)) return false;

            RoomObject = roomObject;

            return true;
        }

        public async Task<bool> SetupRoomObject()
        {
            if (RoomObject == null) return false;

            if (RoomObject.Logic is IFurnitureLogic furnitureLogic)
            {
                if (!await furnitureLogic.Setup(FurnitureDefinition, _furnitureEntity.StuffData)) return false;

                if (furnitureLogic is IFurnitureWiredLogic wiredLogic)
                {
                    wiredLogic.SetupWiredData(_furnitureEntity.WiredData);
                }

                return true;
            }

            return false;
        }

        public void ClearRoomObject()
        {
            if (RoomObject != null)
            {
                RoomObject.Dispose();

                RoomObject = null;
            }
        }

        public async Task<TeleportPairingDto> GetTeleportPairing()
        {
            return await _furnitureManager.GetTeleportPairing(Id);
        }

        public int Id => _furnitureEntity.Id;

        public string Type => "furniture";

        public string LogicType => FurnitureDefinition.Logic;

        public int PlayerId => _furnitureEntity.PlayerEntityId;
    }
}
