using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Security;
using Turbo.Database.Repositories.Player;
using Microsoft.EntityFrameworkCore;

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
            .FindAsync(id);

        public async Task<SecurityTicketEntity> FindByTicketAsync(string ticket) => await _context.SecurityTickets
            .FirstAsync(securityTicket => securityTicket.Ticket == ticket);

        public void DeleteBySecurityTicketEntity(SecurityTicketEntity entity)
        {
            _context.SecurityTickets.Remove(entity);

            _context.SaveChanges();
        }
    }
}
