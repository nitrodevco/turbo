using Microsoft.EntityFrameworkCore;
using System;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Room;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Context
{
    public interface IEmulatorContext : IDisposable
    {
        public DbSet<FurnitureDefinitionEntity> FurnitureDefinitions { get; set; }
        public DbSet<FurnitureEntity> Furnitures { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<RoomBanEntity> RoomBans { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoomModelEntity> RoomModels { get; set; }
        public DbSet<RoomMuteEntity> RoomMutes { get; set; }
        public DbSet<RoomRightEntity> RoomRights { get; set; }
        public DbSet<SecurityTicketEntity> SecurityTickets { get; set; }

        int SaveChanges();
    }
}
