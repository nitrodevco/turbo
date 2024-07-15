using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turbo.Database.Context;
using Turbo.Database.Entities.Furniture;
using Turbo.Core.Database.Dtos;

namespace Turbo.Database.Repositories.Furniture
{
    public class FurnitureRepository(IEmulatorContext _context) : IFurnitureRepository
    {
        public async Task<FurnitureEntity> FindAsync(int id) => await _context.Furnitures
            .FirstOrDefaultAsync(furniture => furniture.Id == id);

        public async Task<List<FurnitureEntity>> FindAllByRoomIdAsync(int roomId) => await _context.Furnitures
            .Where(entity => entity.RoomEntityId == roomId)
            .ToListAsync();

        public async Task<List<FurnitureEntity>> FindAllInventoryByPlayerIdAsync(int playerId) => await _context.Furnitures
            .Where(entity => entity.PlayerEntityId == playerId && entity.RoomEntityId == null)
            .ToListAsync();

        public async Task<TeleportPairingDto> GetTeleportPairingAsync(int furnitureId)
        {
            FurnitureTeleportLinkEntity linkEntity = await FindTeleportLinkByFurnitureIdAsync(furnitureId);

            if (linkEntity == null) return null;

            FurnitureEntity furnitureEntity;

            if (linkEntity.FurnitureEntityOneId == furnitureId)
            {
                furnitureEntity = await FindAsync(linkEntity.FurnitureEntityTwoId);
            }
            else
            {
                furnitureEntity = await FindAsync(linkEntity.FurnitureEntityOneId);
            }

            if (furnitureEntity == null) return null;

            return new TeleportPairingDto
            {
                TeleportId = furnitureEntity.Id,
                RoomId = furnitureEntity.RoomEntityId
            };
        }

        private async Task<FurnitureTeleportLinkEntity> FindTeleportLinkByFurnitureIdAsync(int furnitureId) => await _context.FurnitureTeleportLinks
            .Where(entity => entity.FurnitureEntityOneId == furnitureId || entity.FurnitureEntityTwoId == furnitureId)
            .SingleOrDefaultAsync();
    }
}
