namespace Turbo.Core.Game.Rooms.Object.Logic
{
    public interface IRoomObjectLogicFactory
    {
        public IRoomObjectLogic GetLogic(string type);
    }
}
