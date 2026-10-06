using Microsoft.EntityFrameworkCore;
using Tour_Module.DTOs.Common;
using Tour_Module.DTOs.Tours;
using Tour_Module.Models.Entities;

namespace Tour_Module.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================================================
        // ENTITY TABLES
        // ============================================================

        public DbSet<Location> Locations { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<LocationImage> LocationImages { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<TourAddOn> TourAddOns { get; set; }
        public DbSet<TourDraft> TourDrafts { get; set; }
        public DbSet<TourDraftImage> TourDraftImages { get; set; }

        // ============================================================
        // DTOs (Stored Procedure / Projection Only)
        // ============================================================

        public DbSet<TourDto> TourDtos { get; set; }
        public DbSet<ScalarIntResult> ScalarIntResults { get; set; }
        public DbSet<TourDraftPricingComponent> TourDraftPricingComponents { get; set; }


        // ============================================================
        // MODEL CONFIGURATION
        // ============================================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------------------------------------------------
            // STORED PROCEDURE DTOs (Keyless)
            // ------------------------------------------------------------

            modelBuilder.Entity<TourDto>()
                .HasNoKey()
                .ToView(null);

            modelBuilder.Entity<ScalarIntResult>()
                .HasNoKey()
                .ToView(null);

            // ------------------------------------------------------------
            // ENTITY CONFIGURATIONS
            // ------------------------------------------------------------

            modelBuilder.Entity<TourImage>()
                .HasKey(ti => ti.TourImageId);

            // ------------------------------------------------------------
            // 🔥 CRITICAL FIX — TABLE NAME MISMATCH
            // ------------------------------------------------------------

            modelBuilder.Entity<TourAddOn>()
                .ToTable("TourAllowedOption")          // ✅ EXACT DB TABLE NAME
                .HasKey(x => new { x.TourId, x.OptionId });

            // TourDraft table PK defined.

            modelBuilder.Entity<TourDraft>()
                .HasKey(td => td.DraftId);

            // TourDraftImage table PK defined.

            modelBuilder.Entity<TourDraftImage>()
                .HasKey(x => x.DraftImageId);

            // TourDraftPricingComponent

            modelBuilder.Entity<TourDraftPricingComponent>()
                .HasKey(p => p.PricingComponentId);

            modelBuilder.Entity<TourDraftPricingComponent>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");
        }
    }
}
