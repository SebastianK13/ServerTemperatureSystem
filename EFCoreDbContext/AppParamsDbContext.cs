using System;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using ServerTemperatureSystem.Models;

namespace ServerTemperatureSystem.EFCoreDbContext
{
    public class AppParamsDbContext : DbContext
    {
        public AppParamsDbContext(DbContextOptions<AppParamsDbContext> options) : base(options) { }
        public DbSet<CPU> CPU { get; set; }
        public DbSet<Motherboard> Mobo { get; set; }
        public DbSet<Memory> Memory { get; set; }
        public DbSet<Core> Cores { get; set; }
        public DbSet<UsageDetails> UsageDetails { get; set; }
        public DbSet<TemperatureDetails> TemperatureDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CPU>()
                 .HasMany(u => u.UsageReadings)
                 .WithOne(c => c.CPU);

            modelBuilder.Entity<CPU>()
                .HasMany(t => t.TemperatureReadings)
                .WithOne(c => c.CPU);

            modelBuilder.Entity<Motherboard>()
                .HasMany(t => t.TemperatureReadings)
                .WithOne(m => m.Mobo);

            modelBuilder.Entity<Memory>()
                .HasMany(u => u.UsageReadings)
                .WithOne(m => m.Memory);

            modelBuilder.Entity<Core>()
                .HasMany(u => u.UsageReadings)
                .WithOne(c => c.Core);

            modelBuilder.Entity<Core>()
                .HasMany(t => t.TemperatureReadings)
                .WithOne(c => c.Core);
        }
    }
}