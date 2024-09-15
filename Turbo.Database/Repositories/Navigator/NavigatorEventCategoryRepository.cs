using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Context;
using Turbo.Core.Database.Entities.Navigator;

namespace Turbo.Database.Repositories.Navigator;

public class NavigatorEventCategoryRepository(IEmulatorContext _context) : INavigatorEventCategoryRepository
{
    public async Task<NavigatorEventCategoryEntity> FindAsync(int id)
    {
        return await _context.NavigatorEventCategories.FindAsync(id);
    }

    public async Task<List<NavigatorEventCategoryEntity>> FindAllAsync()
    {
        return await _context.NavigatorEventCategories.ToListAsync();
    }
}