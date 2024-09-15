using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public interface INavigatorEventCategoryRepository : IBaseRepository<NavigatorEventCategoryEntity>
{
    public Task<List<NavigatorEventCategoryEntity>> FindAllAsync();
}