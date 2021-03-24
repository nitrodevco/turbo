using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator
{
    public class NavigatorEventCategoryRepository : INavigatorEventCategoryRepository
    {
        private readonly IEmulatorContext _context;

        public NavigatorEventCategoryRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<NavigatorEventCategoryEntity> FindAsync(int id) => await _context.NavigatorEventCategories.FindAsync(id);

        public async Task<List<NavigatorEventCategoryEntity>> FindAllAsync() => await _context.NavigatorEventCategories.ToListAsync();
    }
}
