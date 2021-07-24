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
        //DbSet<Component> Components { get; set; }
        DbSet<ComponentReadings> Readings { get; set; }
    }
}