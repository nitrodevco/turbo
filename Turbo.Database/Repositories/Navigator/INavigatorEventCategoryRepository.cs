using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public interface INavigatorEventCategoryRepository : IBaseRepository<NavigatorEventCategoryEntity>
{
    public Task<List<NavigatorEventCategoryEntity>> FindAllAsync();
}