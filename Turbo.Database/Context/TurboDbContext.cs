using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Turbo.Core.Configuration;
using Turbo.Database.Attributes;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Catalog;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Navigator;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Room;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Context
{
    public class TurboDbContext : DbContext, IEmulatorContext
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
        public DbSet<PlayerSettingsEntity> PlayerSettings { get; set; }
        public DbSet<RoomBanEntity> RoomBans { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoomModelEntity> RoomModels { get; set; }
        public DbSet<RoomMuteEntity> RoomMutes { get; set; }
        public DbSet<RoomRightEntity> RoomRights { get; set; }
        public DbSet<SecurityTicketEntity> SecurityTickets { get; set; }
        public DbSet<NavigatorCategoryEntity> NavigatorCategories { get; set; }
        public DbSet<NavigatorEventCategoryEntity> NavigatorEventCategories { get; set; }
        public DbSet<NavigatorTabEntity> NavigatorTabs { get; set; }

        public TurboDbContext(DbContextOptions<TurboDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingAddDefaultSqlValues(modelBuilder);

            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", Type.EmptyTypes);

            if (!Directory.Exists("plugins")) Directory.CreateDirectory("plugins");

            var plugins = Directory.GetFiles("plugins", "*.dll");

            List<string> pluginOrder = ["TurboWiredPlugin", "TurboDefaultRevisionPlugin"];

            if (pluginOrder != null) plugins = [.. plugins.OrderBy(value => Array.IndexOf([.. pluginOrder], value))];

            foreach (var plugin in plugins)
            {
                var assembly = Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), plugin));

                var entityTypes = assembly.GetTypes()
                  .Where(t => t.GetCustomAttributes(typeof(TurboEntity), inherit: true).Length != 0);

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, []);
                }
            }
        }

        private void OnModelCreatingAddDefaultSqlValues(ModelBuilder modelBuilder)
        {
            var asm = Assembly.Load("Turbo.Database");

            if (asm == null) return;

            List<Type> types = asm.GetTypes().ToList();

            var dbSets = typeof(TurboDbContext).GetProperties().Where(p => p.PropertyType.Name.ToLower().Contains("dbset")).ToList();

            List<Type> dbSetTypes = new();

            foreach (PropertyInfo pi in dbSets)
            {
                dbSetTypes.Add(pi.PropertyType.GetGenericArguments()[0]);
            }

            foreach (Type t in types)
            {
                if (typeof(Entity).IsAssignableFrom(t) && t.Name != nameof(Entity) && dbSetTypes.Contains(t))
                {
                    var properties = t.GetProperties().ToList();

                    foreach (var p in properties)
                    {
                        var att = p.GetCustomAttribute<DefaultValueSqlAttribute>();

                        if (att != null)
                        {
                            modelBuilder.Entity(t).Property(p.Name).HasDefaultValueSql(att.DefaultValueSql);
                        }
                    }
                }
            }
        }
    }
}
