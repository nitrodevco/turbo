using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture
{
    public interface IFurniture : IRoomObjectFurnitureHolder, IDisposable
    {
        public IFurnitureDefinition FurnitureDefinition { get; }
        public void Save();
        public bool SetRoom(IRoom room);
        public Task<TeleportPairingDto> GetTeleportPairing();
    }
}
