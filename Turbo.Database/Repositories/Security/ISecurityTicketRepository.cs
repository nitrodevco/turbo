using System.Threading.Tasks;
using Turbo.Core.Database.Entities.Security;

namespace Turbo.Database.Repositories.Security;

public interface ISecurityTicketRepository : IBaseRepository<SecurityTicketEntity>
{
    Task<SecurityTicketEntity> FindByTicketAsync(string ticket);
    void DeleteBySecurityTicketEntity(SecurityTicketEntity entity);
}