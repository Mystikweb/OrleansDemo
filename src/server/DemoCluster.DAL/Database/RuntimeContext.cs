using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DemoCluster.DAL.Database
{
    public partial class RuntimeContext : DbContext
    {
        public RuntimeContext(DbContextOptions<RuntimeContext> options)
            : base(options) { }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceEvent> DeviceEvent { get; set; }
        public virtual DbSet<DeviceEventType> DeviceEventType { get; set; }
        public virtual DbSet<DeviceHistory> DeviceHistory { get; set; }
        public virtual DbSet<DeviceSensor> DeviceSensor { get; set; }
        public virtual DbSet<DeviceSensorValue> DeviceSensorValue { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Sensor> Sensor { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer(@"Server=mystikweb.ddns.net,1521;Database=DemoRuntime;User Id=RuntimeManager;Password=MyPa55w0rd!;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "Config");

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.DeviceId).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<DeviceEvent>(entity =>
            {
                entity.HasKey(e => new { e.Device, e.StartTime });
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

            modelBuilder.Entity<DeviceHistory>(entity =>
            {
                entity.HasKey(e => new { e.DeviceId, e.Timestamp });

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceHistory)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceHistory_Device");
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

            modelBuilder.Entity<DeviceSensorValue>(entity =>
            {
                entity.HasKey(e => new { e.DeviceId, e.SensorId, e.Timestamp });

                entity.Property(e => e.SensorId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceSensorValue)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSensorValue_Device");

                entity.HasOne(d => d.Sensor)
                    .WithMany(p => p.DeviceSensorValue)
                    .HasForeignKey(d => d.SensorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSensorValue_Sensor");
            });
        }
    }
}
