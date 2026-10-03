namespace Tour_Module.DTOs.Option
{
    public class TourAddOnDto
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public decimal BasePrice { get; set; }
        public string Currency { get; set; }
        public string Category { get; set; }
        public bool IsDefault { get; set; }

    }
}
