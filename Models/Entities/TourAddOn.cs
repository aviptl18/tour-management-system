namespace Tour_Module.Models.Entities
{
    public class TourAddOn
    {
        public int TourId { get; set; }

        public int OptionId { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}