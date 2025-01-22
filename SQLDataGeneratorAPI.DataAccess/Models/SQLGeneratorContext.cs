using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SQLDataGeneratorAPI.DataAccess.Models;

public partial class SQLGeneratorContext : DbContext
{
    public SQLGeneratorContext()
    {
    }

    public SQLGeneratorContext(DbContextOptions<SQLGeneratorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Country { get; set; }

    public virtual DbSet<FirstName> FirstName { get; set; }

    public virtual DbSet<LastName> LastName { get; set; }

    public virtual DbSet<City> City { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.AlphaCode);

            entity.Property(e => e.AlphaCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumericCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FirstName>(entity =>
        {
            entity.HasKey(e => e.FirstName1);

            entity.Property(e => e.FirstName1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FirstName");
        });

        modelBuilder.Entity<LastName>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.LastName1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LastName");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityName);

            entity.Property(e => e.CityName)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("CityName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
