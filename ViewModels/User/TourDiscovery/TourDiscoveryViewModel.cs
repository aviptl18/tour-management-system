using System.Collections.Generic;
using Tour_Module.DTOs.TourDiscovery;

namespace Tour_Module.ViewModels.User.TourDiscovery
{
    public class TourDiscoveryViewModel
    {
        public TourDiscoveryFilterViewModel Filter { get; set; } = new();

        public List<TourDiscoveryDto> Tours { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public List<string> LocationNames { get; set; } = new();
    }
}