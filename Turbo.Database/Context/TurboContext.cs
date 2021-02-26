using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Turbo.Database.Attributes;

namespace Turbo.Database.Context
{
    public class TurboContext : DbContext, IEmulatorContext
    {
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
