using System.Collections.Generic;

namespace Tour_Module.DTOs.TourDiscovery
{
    public class TourDetailsDto
    {
        public int TourId { get; set; }

        public string TourTitle { get; set; } = string.Empty;

        public string LocationName { get; set; } = string.Empty;

        public string? Overview { get; set; }

        public string? Duration { get; set; }

        public decimal BasePrice { get; set; }

        public decimal IncludedTotal { get; set; }

        public decimal TotalPrice { get; set; }

        public List<string> ImageUrls { get; set; } = new();

        public List<string> AddOns { get; set; } = new();
    }
}