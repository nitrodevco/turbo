using Turbo.Core.Game.Rooms.Utils;

namespace Turbo.Core.Game.Rooms.Mapping;

public interface IRoomModel
{
    public string Model { get; }
    public int TotalX { get; }
    public int TotalY { get; }
    public int TotalSize { get; }

    public IPoint DoorLocation { get; }

    public bool IsValid { get; }

    public int Id { get; }
    public string Name { get; }

    public void Generate();
    public RoomTileState GetTileState(int x, int y);
    public int GetTileHeight(int x, int y);
}