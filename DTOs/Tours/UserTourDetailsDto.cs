using System.Collections.Generic;

namespace Tour_Module.DTOs.Tours
{
    /// <summary>
    /// Public-facing tour details.
    /// This DTO defines what a USER is allowed to see.
    /// </summary>
    public class UserTourDetailsDto
    {
        // =========================================================
        // IDENTITY
        // =========================================================
        public int TourId { get; set; }

        // =========================================================
        // CORE DISPLAY INFO
        // =========================================================
        public string TourTitle { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;

        public string? Overview { get; set; }
        public string? Duration { get; set; }

        // =========================================================
        // PRICING (USER-VISIBLE ONLY)
        // =========================================================
        public decimal BasePrice { get; set; }
        public decimal IncludedTotal { get; set; }
        public decimal TotalPrice { get; set; }

        // =========================================================
        // IMAGES
        // =========================================================
        public string? CoverImageUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new();

        // =========================================================
        // ADD-ONS (DISPLAY ONLY)
        // =========================================================
        public List<string> AddOns { get; set; } = new();
    }
}
