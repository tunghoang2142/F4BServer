using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace F4BBuddyAppWebService.Models;

public partial class F4bContext : DbContext
{
    public F4bContext()
    {
    }

    public F4bContext(DbContextOptions<F4bContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AllStar> AllStars { get; set; }

    public virtual DbSet<Buddy> Buddies { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07532CD7AC");

            entity.ToTable("Account");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LevelCompleted)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AllStar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AllStar__3214EC07A705C155");

            entity.ToTable("AllStar");

            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.School)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Buddy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Buddy__3214EC0727334D25");

            entity.ToTable("Buddy");

            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.GuardianEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsGuardianConsented)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.School)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Buddies)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Buddy__AccountId__3E52440B");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC074AAF4386");

            entity.ToTable("Feedback");

            entity.Property(e => e.AllStarId).HasColumnName("AllStarID");
            entity.Property(e => e.BuddyId).HasColumnName("BuddyID");
            entity.Property(e => e.Feedback1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Feedback");

            entity.HasOne(d => d.AllStar).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AllStarId)
                .HasConstraintName("FK__Feedback__AllSta__412EB0B6");

            entity.HasOne(d => d.Buddy).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.BuddyId)
                .HasConstraintName("FK__Feedback__BuddyI__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
