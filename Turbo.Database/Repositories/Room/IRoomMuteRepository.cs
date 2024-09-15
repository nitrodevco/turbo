using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomMuteRepository : IBaseRepository<RoomMuteEntity>
{
    public Task<List<RoomMuteEntity>> FindAllByRoomIdAsync(int roomId);
    public Task<bool> RemoveMuteEntityAsync(RoomMuteEntity entity);
}