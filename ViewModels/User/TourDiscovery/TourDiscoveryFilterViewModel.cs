namespace Tour_Module.ViewModels.User.TourDiscovery
{
    public class TourDiscoveryFilterViewModel
    {
        public string? SearchTerm { get; set; }

        public string? LocationName { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 12;
    }
}