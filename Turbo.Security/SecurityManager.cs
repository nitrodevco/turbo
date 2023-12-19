using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Security;
using Turbo.Core.Utilities;
using Turbo.Database.Entities.Security;
using Turbo.Database.Repositories.Security;

namespace Turbo.Security
{
    public class SecurityManager : Component, ISecurityManager
    {
        private readonly ISecurityTicketRepository _securityTicketRepository;

        public SecurityManager(ISecurityTicketRepository securityTicketRepository)
        {
            _securityTicketRepository = securityTicketRepository;
        }

        protected override async Task OnInit()
        {
        }

        protected override async Task OnDispose()
        {

        }

        public async Task<int> GetPlayerIdFromTicket(string ticket)
        {
            if ((ticket == null) || (ticket.Length == 0)) return 0;

            var securityTicketEntity = await _securityTicketRepository.FindByTicketAsync(ticket);

            if (securityTicketEntity == null) return 0;
            
            if ((bool)!securityTicketEntity.IsLocked)
            {
                _securityTicketRepository.DeleteBySecurityTicketEntity(securityTicketEntity);
                
                // check timestamp for expiration, if time now is greater than expiration, return 0;
            }

            return securityTicketEntity.PlayerEntityId;
        }
    }
}
