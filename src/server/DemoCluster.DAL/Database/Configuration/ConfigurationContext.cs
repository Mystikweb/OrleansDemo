using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoCluster.DAL.Database.Configuration
{
    public partial class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
            : base(options) { }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceEventType> DeviceEventType { get; set; }
        public virtual DbSet<DeviceSensor> DeviceSensor { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }

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
        }
    }
}
