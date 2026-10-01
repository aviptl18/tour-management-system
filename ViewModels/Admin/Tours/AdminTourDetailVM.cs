using System.Collections.Generic;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class AdminTourDetailVM
    {
        // ------------------------------------------------------------
        // Identity
        // ------------------------------------------------------------
        public int TourId { get; set; }

        // ------------------------------------------------------------
        // Core Tour Information
        // ------------------------------------------------------------
        public string TourTitle { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;

        public string? Overview { get; set; }
        public string? Duration { get; set; }

        // ------------------------------------------------------------
        // Pricing (Admin-defined, read-only here)
        // ------------------------------------------------------------
        public decimal BasePrice { get; set; }
        public decimal IncludedTotal { get; set; }
        public decimal TotalPrice { get; set; }

        // ------------------------------------------------------------
        // Images
        // ------------------------------------------------------------
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// All images associated with this tour (including cover).
        /// Used for gallery view.
        /// </summary>
        public List<string> ImageUrls { get; set; } = new();

        // ------------------------------------------------------------
        // Add-ons (Allowed options for this tour)
        // ------------------------------------------------------------
        public List<string> AddOns { get; set; } = new();
    }
}
