using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class ConfigurationContext : DbContext
    {
        public ConfigurationContext()
        {
        }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceEventType> DeviceEventType { get; set; }
        public virtual DbSet<DeviceSensor> DeviceSensor { get; set; }
        public virtual DbSet<DeviceState> DeviceState { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Monitor> Monitor { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }
        public virtual DbSet<State> State { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseSqlServer("Server=mystikweb.ddns.net,1521;Database=DemoConfiguration;User Id=ConfigManager;Password=MyPa55w0rd123;");
//             }
//         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "Config");

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.DeviceId).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<DeviceEventType>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceEventType)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceEventType_Device");

                entity.HasOne(d => d.EventType)
                    .WithMany(p => p.DeviceEventType)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceEventType_EventType");
            });

            modelBuilder.Entity<DeviceSensor>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceSensor)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSensor_Device");

                entity.HasOne(d => d.Sensor)
                    .WithMany(p => p.DeviceSensor)
                    .HasForeignKey(d => d.SensorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSensor_Sensor");
            });

            modelBuilder.Entity<DeviceState>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceState)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceState_Device");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.DeviceState)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceState_State");
            });

            modelBuilder.Entity<Monitor>(entity =>
            {
                entity.Property(e => e.MonitorId).HasDefaultValueSql("(newsequentialid())");
            });
        }
    }
}
