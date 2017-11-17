using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OrleansDemo.Models
{
    public partial class RuntimeContext : DbContext
    {
        public RuntimeContext(DbContextOptions options)
            : base(options) { }

        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Reading> Readings { get; set; }

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
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Configurations)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Runtime_Configuration_Device");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Reading>(entity =>
            {
                entity.HasAnnotation("SqlServer:MemoryOptimized", true);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });
        }
    }
}
