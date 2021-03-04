using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Turbo.Database.Entities;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Players;

namespace Turbo.Database.Context
{
    public interface IEmulatorContext : IDisposable
    {
        public DbSet<PlayerEntity> Players { get; set; }
        
        // FURNITURE
        public DbSet<FurnitureDefinitionEntity> FurnitureDefinitions { get; set; }
        public DbSet<FurnitureEntity> Furnitures { get; set; }

        int SaveChanges();
    }
}
