using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public interface INavigatorRepository
{
    public Task<NavigatorCategoryEntity> FindNavigatorCategoryAsync(int id);
    public Task<List<NavigatorCategoryEntity>> FindAllNavigatorCategoriesAsync();
    public Task<NavigatorEventCategoryEntity> FindNavigatorEventCategoryAsync(int id);
    public Task<List<NavigatorEventCategoryEntity>> FindAllNavigatorEventCategoriesAsync();
    public Task<NavigatorTabEntity> FindNavigatorTabAsync(int id);
    public Task<List<NavigatorTabEntity>> FindAllNavigatorTabsAsync();
}