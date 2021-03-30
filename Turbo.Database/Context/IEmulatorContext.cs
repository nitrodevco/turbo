using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Navigator;
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
        public DbSet<NavigatorEventCategoryEntity> NavigatorEventCategories { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RankEntity> Ranks { get; set; }
        public DbSet<RankPermissionEntity> RankPermissions { get; set; }
        public DbSet<PlayerPermissionEntity> UserPermissions { get; set; }

        int SaveChanges();
        EntityEntry Update([NotNull] object entity);
        EntityEntry Entry([NotNull] object entity);
        public EntityEntry<TEntity> Entry<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;

    }
}
