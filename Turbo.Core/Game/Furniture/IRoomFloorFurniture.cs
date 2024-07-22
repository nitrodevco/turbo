using System;
using System.Threading.Tasks;
using Turbo.Core.Database.Dtos;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Furniture;

public interface IRoomFloorFurniture : IRoomFurniture, IRoomObjectFloorHolder, IDisposable
{
    public int SavedX { get; }

    public int SavedY { get; }

    public double SavedZ { get; }

    public Rotation SavedRotation { get; }
    public Task<TeleportPairingDto> GetTeleportPairing();
}