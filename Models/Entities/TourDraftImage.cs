namespace Tour_Module.Models.Entities
{
    public class TourDraftImage
    {
        public int DraftImageId { get; set; }
        public int DraftId { get; set; }
        public int LocationImageId { get; set; }
        public bool IsCover { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
