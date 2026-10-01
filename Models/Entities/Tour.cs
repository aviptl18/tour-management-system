using Microsoft.EntityFrameworkCore;

namespace Tour_Module.Models.Entities
{
    public class Tour
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public string TourTitle { get; set; }
        public string? Duration { get; set; }

        [Precision(10, 2)]
        //public decimal? Price { get; set; }
        public string? Overview { get; set; }
        //public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

        public decimal? BasePrice { get; set; }
        public decimal? IncludedTotal { get; set; }
        public decimal? TotalPrice { get; set; }

        public int? CoverImageId { get; set; }

    }
}
