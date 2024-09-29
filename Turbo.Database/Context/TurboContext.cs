using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Database.Attributes;
using Turbo.Core.Database.Entities;
using Turbo.Core.Database.Entities.Catalog;
using Turbo.Core.Database.Entities.Furniture;
using Turbo.Core.Database.Entities.Navigator;
using Turbo.Core.Database.Entities.Players;
using Turbo.Core.Database.Entities.Room;
using Turbo.Core.Database.Entities.Security;
using Turbo.Core.Database.Entities.Tracking;
using Turbo.Database.Attributes;

namespace Turbo.Database.Context;

public class TurboContext(DbContextOptions<TurboContext> options) : DbContext(options), IEmulatorContext
{
    public DbSet<CatalogOfferEntity> CatalogOffers { get; set; }
    public DbSet<CatalogPageEntity> CatalogPages { get; set; }
    public DbSet<CatalogProductEntity> CatalogProducts { get; set; }
    public DbSet<FurnitureDefinitionEntity> FurnitureDefinitions { get; set; }
    public DbSet<FurnitureEntity> Furnitures { get; set; }
    public DbSet<FurnitureTeleportLinkEntity> FurnitureTeleportLinks { get; set; }
    public DbSet<PlayerBadgeEntity> PlayerBadges { get; set; }
    public DbSet<PlayerCurrencyEntity> PlayerCurrencies { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<RoomBanEntity> RoomBans { get; set; }
    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<RoomModelEntity> RoomModels { get; set; }
    public DbSet<RoomMuteEntity> RoomMutes { get; set; }
    public DbSet<RoomRightEntity> RoomRights { get; set; }
    public DbSet<RoomChatlogEntity> Chatlogs { get; set; }
    public DbSet<SecurityTicketEntity> SecurityTickets { get; set; }
    public DbSet<NavigatorTopLevelContextEntity> NavigatorTopLevelContexts { get; set; }
    public DbSet<NavigatorFlatCategoryEntity> NavigatorFlatCategories { get; set; }
    public DbSet<NavigatorEventCategoryEntity> NavigatorEventCategories { get; set; }
    public DbSet<PlayerChatStyleEntity> PlayerChatStyles { get; set; }
    public DbSet<PlayerChatStyleOwnedEntity> PlayerOwnedChatStyles { get; set; }
    public DbSet<PerformanceLogEntity> PerformanceLogs { get; set; }
    public DbSet<PlayerPerksEntity> PlayerPerks { get; set; }
    public DbSet<PlayerFavouriteRoomsEntity> PlayerFavouriteRooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerFavouriteRoomsEntity>()
            .HasKey(p => new { p.PlayerId, p.RoomId });

        modelBuilder.Entity<PlayerFavouriteRoomsEntity>()
            .HasOne(p => p.Player)
            .WithMany()
            .HasForeignKey(p => p.PlayerId);

        modelBuilder.Entity<PlayerFavouriteRoomsEntity>()
            .HasOne(p => p.Room)
            .WithMany()
            .HasForeignKey(p => p.RoomId);

        OnModelCreatingAddDefaultSqlValues(modelBuilder);

        modelBuilder.Entity<CatalogPageEntity>(entity =>
        {
            entity.Property(e => e.ImageData)
                .HasColumnType("json");

            entity.Property(e => e.TextData)
                .HasColumnType("json");
        });

        var entityMethod = typeof(ModelBuilder).GetMethod("Entity", Type.EmptyTypes);

        if (!Directory.Exists("plugins")) Directory.CreateDirectory("plugins");

        var plugins = Directory.GetFiles("plugins", "*.dll");

        foreach (var plugin in plugins)
        {
            // Load assembly
            var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), plugin));

            var entityTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TurboEntity), true).Any());

            foreach (var type in entityTypes)
                entityMethod.MakeGenericMethod(type)
                    .Invoke(modelBuilder, new object[] { });
        }
    }

    private void OnModelCreatingAddDefaultSqlValues(ModelBuilder modelBuilder)
    {
        var asm = Assembly.Load("Turbo.Core");

        if (asm == null) return;

        var types = asm.GetTypes().ToList();

        var dbSets = typeof(TurboContext).GetProperties().Where(p => p.PropertyType.Name.ToLower().Contains("dbset"))
            .ToList();

        List<Type> dbSetTypes = new();

        foreach (var pi in dbSets) dbSetTypes.Add(pi.PropertyType.GetGenericArguments()[0]);

        foreach (var t in types)
        {
            if (!typeof(Entity).IsAssignableFrom(t) || t.Name == nameof(Entity) || !dbSetTypes.Contains(t)) continue;

            var properties = t.GetProperties().ToList();

            foreach (var p in properties)
            {
                var att = p.GetCustomAttribute<DefaultValueSqlAttribute>();

                if (att != null) modelBuilder.Entity(t).Property(p.Name).HasDefaultValueSql(att.Value?.ToString());
            }
        }
    }
}