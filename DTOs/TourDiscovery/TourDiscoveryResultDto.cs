using System.Collections.Generic;

namespace Tour_Module.DTOs.TourDiscovery
{
    public class TourDiscoveryResultDto
    {
        public List<TourDiscoveryDto> Tours { get; set; } = new();

        public int TotalCount { get; set; }
    }
}
