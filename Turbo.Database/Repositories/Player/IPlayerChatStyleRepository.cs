using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Players;

namespace Turbo.Database.Repositories.ChatStyles;

public interface IPlayerChatStyleRepository : IBaseRepository<PlayerChatStyleEntity>
{
    public Task<List<PlayerChatStyleEntity>> FindAllAsync();
}