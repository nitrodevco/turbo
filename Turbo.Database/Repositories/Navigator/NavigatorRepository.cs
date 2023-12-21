using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator
{
    public class NavigatorRepository : INavigatorRepository
    {
        private readonly IEmulatorContext _context;

        public NavigatorRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<NavigatorCategoryEntity> FindNavigatorCategoryAsync(int id) => await _context.NavigatorCategories.FindAsync(id);

        public async Task<List<NavigatorCategoryEntity>> FindAllNavigatorCategoriesAsync() => await _context.NavigatorCategories.ToListAsync();

        public async Task<NavigatorEventCategoryEntity> FindNavigatorEventCategoryAsync(int id) => await _context.NavigatorEventCategories.FindAsync(id);

        public async Task<List<NavigatorEventCategoryEntity>> FindAllNavigatorEventCategoriesAsync() => await _context.NavigatorEventCategories.ToListAsync();

        public async Task<NavigatorTabEntity> FindNavigatorTabAsync(int id) => await _context.NavigatorTabs.FindAsync(id);

        public async Task<List<NavigatorTabEntity>> FindAllNavigatorTabsAsync() => await _context.NavigatorTabs.ToListAsync();
    }
}
