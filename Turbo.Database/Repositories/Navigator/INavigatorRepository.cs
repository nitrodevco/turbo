using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public interface INavigatorRepository
{
    Task<NavigatorFlatCategoryEntity> FlatCategoryEntityByIdAsync(int categoryId);
    Task<List<NavigatorFlatCategoryEntity>> GetFlatCategoriesAsync();
    Task<List<NavigatorEventCategoryEntity>> GetEventCategoriesAsync();
    Task<List<NavigatorTopLevelContextEntity>> GetTopLevelContextsAsync();
}