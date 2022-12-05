using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DealerPeak.Models;

public partial class DealerPeakDbContext : DbContext
{
    public DealerPeakDbContext()
    {
    }

    public DealerPeakDbContext(DbContextOptions<DealerPeakDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dealer> Dealers { get; set; }

    public virtual DbSet<DealerVehicle> DealerVehicles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-6BP2OJF\\SQLEXPRESS;Database=DealerPeakDB;Integrated Security= SSPI;trustServerCertificate=yes; user id=DESKTOP-6BP2OJF\\bhavi; Trusted_Connection=True; MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dealer>(entity =>
        {
            entity.Property(e => e.DealerId).HasColumnName("DealerID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.DealerContact)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DealerEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DealerName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DealerVehicle>(entity =>
        {
            entity.HasKey(e => new { e.DealerVehicleId, e.DealerId, e.VehicleId });

            entity.ToTable("Dealer_Vehicle");

            entity.Property(e => e.DealerVehicleId).HasColumnName("DealerVehicleID");
            entity.Property(e => e.DealerId).HasColumnName("DealerID");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Dealer).WithMany(p => p.DealerVehicles)
                .HasForeignKey(d => d.DealerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dealer_Vehicle_Dealers");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.DealerVehicles)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dealer_Vehicle_Vehicles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Contact, "IX_Users").IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnName("UserID");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contact)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                  .HasDefaultValueSql("1")
                  .HasColumnName("isActive");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LoginType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => e.Vin, "IX_Vehicles").IsUnique();

            entity.Property(e => e.VehicleId)
                .HasColumnName("VehicleID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.VehicleMakeYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehiclePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NewVehicle)
                  .HasDefaultValueSql("1")
                  .HasColumnName("newVehicle");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("VIN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
