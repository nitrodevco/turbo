using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Turbo.Core.Security;
using Turbo.Core.Utilities;
using Turbo.Database.Repositories.Security;

namespace Turbo.Security;

public class SecurityManager(IServiceScopeFactory _serviceScopeFactory) : Component, ISecurityManager
{
    public async Task<int> GetPlayerIdFromTicket(string ticket)
    {
        if (ticket == null || ticket.Length == 0) return 0;

        using var scope = _serviceScopeFactory.CreateScope();
        var securityTicketRepository = scope.ServiceProvider.GetService<ISecurityTicketRepository>();
        var securityTicketEntity = await securityTicketRepository.FindByTicketAsync(ticket);

        if (securityTicketEntity == null) return 0;

        if ((bool)!securityTicketEntity.IsLocked)
            securityTicketRepository.DeleteBySecurityTicketEntity(securityTicketEntity);
        // check timestamp for expiration, if time now is greater than expiration, return 0;
        return securityTicketEntity.PlayerEntityId;
    }

    protected override async Task OnInit()
    {
    }

    protected override async Task OnDispose()
    {
    }
}