using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Entities.Security;
using Turbo.Database.Context;

namespace Turbo.Database.Repositories.Security;

public class SecurityTicketRepository(IEmulatorContext _context) : ISecurityTicketRepository
{
    public async Task<SecurityTicketEntity> FindAsync(int id)
    {
        return await _context.SecurityTickets
            .FirstOrDefaultAsync(securityTicket => securityTicket.Id == id);
    }

    public async Task<SecurityTicketEntity> FindByTicketAsync(string ticket)
    {
        return await _context.SecurityTickets
            .FirstOrDefaultAsync(securityTicket => securityTicket.Ticket == ticket);
    }

    public void DeleteBySecurityTicketEntity(SecurityTicketEntity entity)
    {
        _context.SecurityTickets.Remove(entity);

        _context.SaveChanges();
    }
}