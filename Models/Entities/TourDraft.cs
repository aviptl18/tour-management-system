namespace Tour_Module.Models.Entities
{
    public class TourDraft
    {
        public int DraftId { get; set; }
        public string TourTitle { get; set; }
        public string Overview { get; set; }

        public int? LocationId { get; set; }
        public string? Duration { get; set; }
        public decimal? BasePrice { get; set; }

        public int DraftStep { get; set; }
        public string Status { get; set; }

        public int? OriginalTourId { get; set; }

        public int CreatedByAdminId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
