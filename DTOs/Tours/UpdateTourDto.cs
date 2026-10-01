namespace Tour_Module.DTOs.Tours
{
    public class UpdateTourDto
    {
        public int TourId { get; set; }
        public int LocationId { get; set; }
        public string TourTitle { get; set; }
        public string? Duration { get; set; }
        public string? Overview { get; set; }
        //public string? ImageUrl { get; set; }
    }
}
