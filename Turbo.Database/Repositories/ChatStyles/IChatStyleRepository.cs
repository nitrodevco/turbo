using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.ChatStyles;

namespace Turbo.Database.Repositories.ChatStyles;

public interface IChatStyleRepository : IBaseRepository<ChatStyleEntity>
{
    Task<List<ChatStyleEntity>> GetAllAsync();
}