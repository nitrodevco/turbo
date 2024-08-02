using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Tracking;

namespace Turbo.Database.Repositories.Tracking;

public class PerformanceLogRepository(IEmulatorContext _context) : IPerformanceLogRepository
{
    public async Task<PerformanceLogEntity> FindAsync(int id)
    {
        return await _context.PerformanceLogs
            .FindAsync(id);
    }
    
    public async Task AddAsync(PerformanceLogEntity entity)
    {
        _context.PerformanceLogs.Add(entity);
        await _context.SaveChangesAsync();
    }
}