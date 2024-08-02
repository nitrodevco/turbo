using System.Threading.Tasks;
using Turbo.Database.Entities.Tracking;

namespace Turbo.Database.Repositories.Tracking;

public interface IPerformanceLogRepository : IBaseRepository<PerformanceLogEntity>
{
    Task AddAsync(PerformanceLogEntity entity);
}