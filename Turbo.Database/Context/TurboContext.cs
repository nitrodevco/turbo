using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Navigator;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Room;
using Turbo.Database.Entities.Security;
using Turbo.Database.Entities.Tracking;

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
    public DbSet<NavigatorCategoryEntity> NavigatorCategories { get; set; }
    public DbSet<NavigatorEventCategoryEntity> NavigatorEventCategories { get; set; }
    public DbSet<NavigatorTabEntity> NavigatorTabs { get; set; }
    public DbSet<PlayerChatStyleEntity> PlayerChatStyles { get; set; }
    public DbSet<PlayerChatStyleOwnedEntity> PlayerOwnedChatStyles { get; set; }
    public DbSet<PerformanceLogEntity> PerformanceLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingAddDefaultSqlValues(modelBuilder);

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
        var asm = Assembly.Load("Turbo.Database");

        if (asm == null) return;

        var types = asm.GetTypes().ToList();

        var dbSets = typeof(TurboContext).GetProperties().Where(p => p.PropertyType.Name.ToLower().Contains("dbset"))
            .ToList();

        List<Type> dbSetTypes = new();

        foreach (var pi in dbSets) dbSetTypes.Add(pi.PropertyType.GetGenericArguments()[0]);

        foreach (var t in types)
            if (typeof(Entity).IsAssignableFrom(t) && t.Name != nameof(Entity) && dbSetTypes.Contains(t))
            {
                var properties = t.GetProperties().ToList();

                foreach (var p in properties)
                {
                    var att = p.GetCustomAttribute<DefaultValueSqlAttribute>();

                    if (att != null) modelBuilder.Entity(t).Property(p.Name).HasDefaultValueSql(att.DefaultValueSql);
                }
            }
    }
}