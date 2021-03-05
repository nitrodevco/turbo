using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Repositories.Security
{
    public interface ISecurityTicketRepository : IBaseRepository<SecurityTicketEntity>
    {
        Task<SecurityTicketEntity> FindByTicketAsync(string ticket);
        void DeleteBySecurityTicketEntity(SecurityTicketEntity entity);
    }
}
