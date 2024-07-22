using System.Threading.Tasks;
using Turbo.Core.Game;
using Turbo.Core.Game.Rooms;

namespace Turbo.Rooms.Cycles;

public abstract class RoomCycle(IRoom _room) : ICyclable
{
    public abstract Task Cycle();
}