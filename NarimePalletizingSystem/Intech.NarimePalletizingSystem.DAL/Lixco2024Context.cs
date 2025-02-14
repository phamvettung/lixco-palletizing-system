using System;
using System.Collections.Generic;
using Intech.NarimePalletizingSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Intech.NarimePalletizingSystem.DAL
{
    public partial class Lixco2024Context : DbContext
    {
        public Lixco2024Context()
        {
        }

        public Lixco2024Context(DbContextOptions<Lixco2024Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Pallet> Pallets { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
                var connectionString = configuration["connectionStrings:dbDefault"];
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pallet>(entity =>
            {
                entity.ToTable("pallets");

                entity.Property(e => e.PalletId).HasColumnName("pallet_id");

                entity.Property(e => e.Craft).HasColumnName("craft");

                entity.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("date_time");

                entity.Property(e => e.FormName)
                    .HasMaxLength(50)
                    .HasColumnName("form_name");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(100)
                    .HasColumnName("group_code");

                entity.Property(e => e.LeverCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lever_code");

                entity.Property(e => e.LineId).HasColumnName("line_id");

                entity.Property(e => e.Lot)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lot");

                entity.Property(e => e.NumOrder).HasColumnName("num_order");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_code");

                entity.Property(e => e.Reached).HasColumnName("reached");

                entity.Property(e => e.ShiftId).HasColumnName("shift_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.HasOne(d => d.ProductCodeNavigation)
                    .WithMany(p => p.Pallets)
                    .HasForeignKey(d => d.ProductCode)
                    .HasConstraintName("FK__pallets__product__05D8E0BE");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode)
                    .HasName("PK__products__AE1A8CC5B23F6745");

                entity.ToTable("products");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_code");

                entity.Property(e => e.Model).HasColumnName("model");

                entity.Property(e => e.NetWeight).HasColumnName("net_weight");

                entity.Property(e => e.NumBinsOnPallet).HasColumnName("num_bins_on_pallet");

                entity.Property(e => e.NumPacketsOnBin).HasColumnName("num_packets_on_bin");

                entity.Property(e => e.PacketWeight).HasColumnName("packet_weight");

                entity.Property(e => e.ProductImages)
                    .HasMaxLength(1)
                    .HasColumnName("product_images");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .HasColumnName("product_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
