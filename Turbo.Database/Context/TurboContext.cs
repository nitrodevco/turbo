using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Turbo.Database.Attributes;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Players;
using Turbo.Database.Entities.Security;

namespace Turbo.Database.Context
{
    public class TurboContext : DbContext, IEmulatorContext
    {
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<SecurityTicketEntity> SecurityTickets { get; set; }

        public DbSet<FurnitureDefinitionEntity> FurnitureDefinitions { get; set; }
        public DbSet<FurnitureEntity> Furnitures { get; set; }

        public TurboContext(DbContextOptions<TurboContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityMethod = typeof(ModelBuilder).GetMethod("Entity", Type.EmptyTypes);

            if (!Directory.Exists("plugins"))
            {
                Directory.CreateDirectory("plugins");
            }

            var plugins = Directory.GetFiles("plugins", "*.dll");

            foreach (var plugin in plugins)
            {
                // Load assembly
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), plugin));

                var entityTypes = assembly.GetTypes()
                  .Where(t => t.GetCustomAttributes(typeof(TurboEntity), inherit: true).Any());

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }
            }
        }
    }
}
