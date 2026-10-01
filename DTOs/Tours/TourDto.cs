namespace Tour_Module.DTOs.Tours
{
    public class TourDto
    {
        // ---------------------------
        // Identity
        // ---------------------------
        public int TourId { get; set; }

        // ---------------------------
        // Location
        // ---------------------------
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;

        // ---------------------------
        // Tour Info
        // ---------------------------
        public string TourTitle { get; set; } = string.Empty;
        public string? Duration { get; set; }
        public string? Overview { get; set; }

        // ---------------------------
        // Pricing (NEW ARCHITECTURE)
        // ---------------------------
        public decimal? BasePrice { get; set; }
        public decimal? TotalPrice { get; set; }

        // ---------------------------
        // Images
        // ---------------------------
        public string? CoverImageUrl { get; set; }
    }
}
