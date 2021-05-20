using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ChargingPointApi.Models;

#nullable disable

namespace ChargingPointApi.Data
{
    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext()
        {
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChargingPoint> ChargingPoints { get; set; }
        public virtual DbSet<Heartbeat> Heartbeats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:jesper-sqldatabase.database.windows.net,1433;Initial Catalog=CSMSDB;Persist Security Info=False;User ID=SqlAdmin;Password=kaliberSNUS1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChargingPoint>(entity =>
            {
                entity.ToTable("ChargingPoint");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ip")
                    .IsFixedLength(true);

                entity.Property(e => e.Port)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("port")
                    .IsFixedLength(true);

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("serialNumber");

                entity.Property(e => e.SocketId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Heartbeat>(entity =>
            {
                entity.ToTable("Heartbeat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cpid).HasColumnName("CPId");

                entity.Property(e => e.Hbtime).HasColumnName("HBtime");

                entity.HasOne(d => d.Cp)
                    .WithMany(p => p.Heartbeats)
                    .HasForeignKey(d => d.Cpid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Heartbeat_ChargePoint");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
