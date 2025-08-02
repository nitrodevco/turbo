using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms;

namespace Turbo.Core.Game.Furniture;

public interface IRoomFurniture
{
    public IFurnitureDefinition FurnitureDefinition { get; }
    public string LogicType { get; }
    public void Save();
    public void SetRoom(IRoom room);
    public bool SetPlayer(IPlayer player);
    public bool SetPlayer(int playerId, string playerName = "");
}