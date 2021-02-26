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

        public Habbo Find(int id) => _context.Habbos.Find(id);
    }
}
