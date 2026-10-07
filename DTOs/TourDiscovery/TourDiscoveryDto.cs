namespace Tour_Module.DTOs.TourDiscovery
{
    public class TourDiscoveryDto
    {
        public int TourId { get; set; }

        public string TourTitle { get; set; } = string.Empty;

        public int LocationId { get; set; }

        public string LocationName { get; set; } = string.Empty;

        public string? Duration { get; set; }

        public decimal? BasePrice { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? CoverImageUrl { get; set; }
    }
}
