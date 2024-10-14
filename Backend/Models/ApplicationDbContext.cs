using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetStockAPI.Models;

public partial class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Category> categories { get; set; }

    public virtual DbSet<Product> products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.categoryid).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.categoryname).HasMaxLength(64);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.productid).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.modifieddate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.productname).HasMaxLength(128);
            entity.Property(e => e.productpicture).HasMaxLength(256);
            entity.Property(e => e.unitprice).HasPrecision(18, 2);
        });
        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
