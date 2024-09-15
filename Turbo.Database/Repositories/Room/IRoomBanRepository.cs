using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room;

public interface IRoomBanRepository : IBaseRepository<RoomBanEntity>
{
    public Task<List<RoomBanEntity>> FindAllByRoomIdAsync(int roomId);
    public Task<bool> BanPlayerIdAsync(int roomId, int playerId, DateTime expiration);
    public Task<bool> RemoveBanEntityAsync(RoomBanEntity entity);
}