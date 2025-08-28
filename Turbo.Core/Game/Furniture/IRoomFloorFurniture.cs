using System;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Furniture;

public interface IRoomFloorFurniture : IRoomFurniture, IRoomObjectFloorHolder, IDisposable
{
    int SavedX { get; }
    int SavedY { get; }
    double SavedZ { get; }
    Rotation SavedRotation { get; }
    Task<TeleportPairingDto> GetTeleportPairing();
}