using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Navigator;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Navigator;

public class NavigatorRepository(IEmulatorContext _context) : INavigatorRepository
{
    public async Task<List<NavigatorFlatCategoryEntity>> GetFlatCategoriesAsync() => await _context.NavigatorFlatCategories
        .AsNoTracking()
        .Where(c => c.Visible)
        .OrderBy(c => c.OrderNum)
        .ToListAsync();

    public async Task<List<NavigatorEventCategoryEntity>> GetEventCategoriesAsync() => await _context.NavigatorEventCategories
        .AsNoTracking()
        .Where(c => c.Visible)
        .ToListAsync();

    public async Task<List<NavigatorTopLevelContextEntity>> GetTopLevelContextsAsync() => await _context.NavigatorTopLevelContexts
        .AsNoTracking()
        .Where(c => c.Visible)
        .OrderBy(c => c.OrderNum)
        .ToListAsync();
}