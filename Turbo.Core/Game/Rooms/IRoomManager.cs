using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Storage;

namespace Turbo.Core.Game.Rooms
{
    public interface IRoomManager : IAsyncInitialisable, IAsyncDisposable, ICyclable
    {
        public Task<IRoom> GetRoom(int id);
        public IRoom GetOnlineRoom(int id);
        public Task<IRoom> GetOfflineRoom(int id);
        public ValueTask RemoveRoom(int id);
        public IRoomModel GetModel(int id);
        public IRoomModel GetModelByName(string name);

        public IStorageQueue StorageQueue { get; }
    }
}
