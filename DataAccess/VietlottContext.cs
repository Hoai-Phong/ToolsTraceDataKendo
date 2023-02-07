using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Vietlott.DataAccess.Entities;

namespace Vietlott.DataAccess;

public partial class VietlottContext : DbContext
{
    public VietlottContext()
    {
    }

    public VietlottContext(DbContextOptions<VietlottContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GuessResultKeno> GuessResultKenos { get; set; }

    public virtual DbSet<KenoResult> KenoResults { get; set; }

    public virtual DbSet<ResultKeno> ResultKenos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=192.168.200.18;Database=vietlott;User Id=vt001;Password=I00tv;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuessResultKeno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GuessResult_Kendo");

            entity.ToTable("GuessResult_Keno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LiveKy).HasColumnName("Live_ky");
        });

        modelBuilder.Entity<KenoResult>(entity =>
        {
            entity.HasKey(e => e.Ky);

            entity.ToTable("KenoResult");

            entity.Property(e => e.Ky).ValueGeneratedNever();
            entity.Property(e => e.Result)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ResultTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ResultKeno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Result_Kendo");

            entity.ToTable("Result_Keno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LiveDate)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("Live_date");
            entity.Property(e => e.LiveKy).HasColumnName("Live_ky");
            entity.Property(e => e.NextKy).HasColumnName("Next_ky");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
