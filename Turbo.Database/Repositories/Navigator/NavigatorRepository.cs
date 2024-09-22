using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public class NavigatorRepository(IEmulatorContext _context) : INavigatorRepository
{
    public async Task<List<NavigatorFlatCategoryEntity>> GetFlatCategoriesAsync()
    {
        return await _context.NavigatorFlatCategories
            .AsNoTracking()
            .Where(c => c.Visible)
            .OrderBy(c => c.OrderNum)
            .ToListAsync();
    }
    
    public async Task<List<NavigatorEventCategoryEntity>> GetEventCategoriesAsync()
    {
        return await _context.NavigatorEventCategories
            .AsNoTracking()
            .Where(c => c.Visible)
            .ToListAsync();
    }

    public async Task<List<NavigatorTopLevelContextEntity>> GetTopLevelContextsAsync()
    {
        return await _context.NavigatorTopLevelContexts
            .AsNoTracking()
            .Where(c => c.Visible)
            .OrderBy(c => c.OrderNum)
            .ToListAsync();
    }
}