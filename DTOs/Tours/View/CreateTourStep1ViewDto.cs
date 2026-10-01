namespace Tour_Module.DTOs.Tours.View
{
    public class CreateTourStep1ViewDto
    {
        public int DraftId { get; set; }
        public string TourTitle { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public int? OriginalTourId { get; set; }

    }
}
