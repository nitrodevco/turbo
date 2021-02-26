using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities;

namespace Turbo.Database.Repositories
{
    public class HabboRepository : IHabboRepository
    {
        private readonly IEmulatorContext _context;

        public HabboRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<Habbo> FindAsync(int id) => await _context.Habbos.FindAsync(id);
    }
}
