using System;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Core.Game.Rooms.Object;

namespace Turbo.Core.Game.Furniture
{
    public interface IRoomFloorFurniture : IRoomFurniture, IRoomObjectFloorHolder, IDisposable
    {
        public Task<TeleportPairingDto> GetTeleportPairing();

        public int SavedX { get; }

        public int SavedY { get; }

        public double SavedZ { get; }

        public Rotation SavedRotation { get; }
    }
}
