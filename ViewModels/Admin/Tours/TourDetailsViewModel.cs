using System.Collections.Generic;

namespace Tour_Module.ViewModels.Admin.Tours
{
    public class TourDetailsViewModel
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
        // Pricing
        // ------------------------------------------------------------
        public decimal BasePrice { get; set; }

        public decimal IncludedTotal { get; set; }

        public decimal TotalPrice { get; set; }

        // ------------------------------------------------------------
        // Images
        // ------------------------------------------------------------
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// All images associated with this tour, including the cover image.
        /// </summary>
        public List<string> ImageUrls { get; set; } = new();

        // ------------------------------------------------------------
        // Add-ons
        // ------------------------------------------------------------
        public List<string> AddOns { get; set; } = new();
    }
}