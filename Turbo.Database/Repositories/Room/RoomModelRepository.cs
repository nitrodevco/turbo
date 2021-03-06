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
    public class RoomModelRepository : IRoomModelRepository
    {
        private readonly IEmulatorContext _context;

        public RoomModelRepository(IEmulatorContext context)
        {
            _context = context;
        }

        public async Task<RoomModelEntity> FindAsync(int id) => await _context.RoomModels
            .FindAsync(id);

        public async Task<RoomModelEntity> FindByNameAsync(string name) => await _context.RoomModels
            .FirstAsync(roomModel => roomModel.Name == name);

        public async Task<List<RoomModelEntity>> FindAllAsync() => await _context.RoomModels
            .ToListAsync();
    }
}
