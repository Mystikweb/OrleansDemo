using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrleansDemo.API.Models
{
    public partial class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<Reading> Readings { get; set; }
        public virtual DbSet<ReadingType> ReadingTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer(@"Data Source=MAK000027;Initial Catalog=OrleansDemoDb;Integrated Security=True");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.DeviceTypeId })
                    .HasName("IX_Configuration_Device_Name_DeviceTypeId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DeviceType)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.DeviceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Device_DeviceType");
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Reading>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Reading_Device");

                entity.HasOne(d => d.ReadingType)
                    .WithMany(p => p.Readings)
                    .HasForeignKey(d => d.ReadingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Configuration_Reading_ReadingType");
            });
        }
    }
}
