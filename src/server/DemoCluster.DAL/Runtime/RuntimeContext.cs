using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoCluster.DAL.Runtime
{
    public partial class RuntimeContext : DbContext
    {
        public virtual DbSet<DeviceEvent> DeviceEvent { get; set; }
        public virtual DbSet<DeviceSensorValue> DeviceSensorValue { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//             if (!optionsBuilder.IsConfigured)
//             {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseSqlServer(@"Server=mystikweb.ddns.net,1521;Database=DemoRuntime;User Id=RuntimeManager;Password=MyPa55w0rd!;");
//             }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "Runtime");

            modelBuilder.Entity<DeviceEvent>(entity =>
            {
                entity.HasKey(e => new { e.Device, e.StartTime });
            });

            modelBuilder.Entity<DeviceSensorValue>(entity =>
            {
                entity.HasKey(e => new { e.Device, e.Sensor, e.Timestamp });
            });
        }
    }
}
