using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoomManager : IRoomContainer, IAsyncInitialisable, IAsyncDisposable, ICyclable
    {
        public IRoomModel GetModel(int id);
        public IRoomModel GetModelByName(string name);
        public Task<IRoom> GetOfflineRoom(int id);
    }
}
