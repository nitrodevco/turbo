using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Entities.Security;
using Turbo.Database.Repositories.Security;

namespace Turbo.Security
{
    public class SecurityManager : ISecurityManager
    {
        private readonly ISecurityTicketRepository _securityTicketRepository;
        public SecurityManager(ISecurityTicketRepository securityTicketRepository)
        {
            _securityTicketRepository = securityTicketRepository;
        }

        public async ValueTask InitAsync()
        {
            
        }

        public async ValueTask DisposeAsync()
        {
            
        }

        public async Task<int> GetPlayerIdFromTicket(string ticket)
        {
            if ((ticket == null) || (ticket.Length == 0)) return 0;

            SecurityTicketEntity securityTicketEntity = await _securityTicketRepository.FindByTicketAsync(ticket);

            if (securityTicketEntity == null) return 0;

            if(!securityTicketEntity.IsLocked)
            {
                _securityTicketRepository.DeleteBySecurityTicketEntity(securityTicketEntity);

                // check timestamp for expiration, if time now is greater than expiration, return 0;
            }

            return securityTicketEntity.PlayerEntity.Id;
        }
    }
}
