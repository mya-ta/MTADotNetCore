using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MTADotNetCore.ConsoleApp.Models;

public partial class DotNetTrainingB4AppDbContext : DbContext
{
    public DotNetTrainingB4AppDbContext()
    {
    }

    public DotNetTrainingB4AppDbContext(DbContextOptions<DotNetTrainingB4AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBlog> TblBlogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=DotNetTrainingB4;User ID=sa;Password=sa@123;TrustServerCertificate=True;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBlog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_blog");

            entity.Property(e => e.BlogAuthor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BlogContent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BlogId).ValueGeneratedOnAdd();
            entity.Property(e => e.BlogTitle)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}