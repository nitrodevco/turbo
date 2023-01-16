using Turbo.Core.Game.Furniture.Definition;
using Turbo.Core.Game.Rooms;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Furniture
{
    public interface IRoomFurniture
    {
        public IFurnitureDefinition FurnitureDefinition { get; }
        public void Save();
        public bool SetRoom(IRoom room);
        public void ClearRoom();
        public bool SetPlayer(IPlayer player);
        public bool SetPlayer(int playerId, string playerName = "");
        public string LogicType { get; }
    }
}