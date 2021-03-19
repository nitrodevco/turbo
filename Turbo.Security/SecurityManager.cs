using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Turbo.Core.Security;
using Turbo.Database.Entities.Security;
using Turbo.Database.Repositories.Security;

namespace Turbo.Security
{
    public class SecurityManager : ISecurityManager
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SecurityManager(IServiceScopeFactory scopeFactory)
        {
            _serviceScopeFactory = scopeFactory;
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

            SecurityTicketEntity securityTicketEntity;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var securityTicketRepository = scope.ServiceProvider.GetService<ISecurityTicketRepository>();

                securityTicketEntity = await securityTicketRepository.FindByTicketAsync(ticket);

                if (securityTicketEntity == null) return 0;

                if (!securityTicketEntity.IsLocked)
                {
                    securityTicketRepository.DeleteBySecurityTicketEntity(securityTicketEntity);

                    // check timestamp for expiration, if time now is greater than expiration, return 0;
                }
            }

            return securityTicketEntity.PlayerEntityId;
        }
    }
}
