using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DataPoint> DataPoints { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=mssqlstud.fhict.local;Database=dbi538068_MotoForce;User Id=dbi538068_MotoForce;Password=Dungprotfi8;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DataPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__data_poi__3213E83F105D1AAB");

            entity.ToTable("data_point");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Acceleration).HasColumnName("acceleration");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.LateralG).HasColumnName("lateral_g");
            entity.Property(e => e.Lean).HasColumnName("lean");
            entity.Property(e => e.Lon).HasColumnName("lon");
            entity.Property(e => e.RouteId).HasColumnName("route_id");
            entity.Property(e => e.Speed).HasColumnName("speed");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Route).WithMany(p => p.DataPoints)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_route_data_point");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__route__3213E83F0D1943D9");

            entity.ToTable("route");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Distance).HasColumnName("distance");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Routes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_route");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83FDD5C49D0");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ__user__AB6E6164936F5519").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.ProfilePic).HasColumnName("profile_pic");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
