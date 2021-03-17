using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Repositories.Security
{
    public class SecurityTicketRepository : ISecurityTicketRepository
    {
        private readonly IEmulatorContext _context;

        public SecurityTicketRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<SecurityTicketEntity> FindAsync(int id) => await _context.SecurityTickets
            .FirstOrDefaultAsync(securityTicket => securityTicket.Id == id);

        public async Task<SecurityTicketEntity> FindByTicketAsync(string ticket) => await _context.SecurityTickets
            .FirstOrDefaultAsync(securityTicket => securityTicket.Ticket == ticket);

        public void DeleteBySecurityTicketEntity(SecurityTicketEntity entity)
        {
            _context.SecurityTickets.Remove(entity);

            _context.SaveChanges();
        }
    }
}
