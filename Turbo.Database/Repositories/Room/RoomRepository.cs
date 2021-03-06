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
using Turbo.Database.Entities.Room;

namespace Turbo.Database.Repositories.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IEmulatorContext _context;

        public RoomRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomEntity> FindAsync(int id) => await _context.Rooms
            .FindAsync(id);
    }
}
