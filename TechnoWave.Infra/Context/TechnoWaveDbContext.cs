using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TechnoWave.Infra.Context;

public partial class TechnoWaveDbContext : DbContext
{
    public TechnoWaveDbContext()
    {
    }

    public TechnoWaveDbContext(DbContextOptions<TechnoWaveDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SK;Database=TechnoWave;Integrated Security=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__TblUsers__1788CC4C61D25F62");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
